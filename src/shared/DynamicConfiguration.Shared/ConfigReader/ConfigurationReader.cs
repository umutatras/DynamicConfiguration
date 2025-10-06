using DynamicConfiguration.Shared.ConfigReader.Events;
using DynamicConfiguration.Shared.ConfigReader.Models;
using Microsoft.Data.SqlClient;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using IModel = RabbitMQ.Client.IModel;
using Timer = System.Timers.Timer;
namespace DynamicConfiguration.Shared.ConfigReader;


public class ConfigurationReader : IDisposable
{
    private readonly string _applicationName;
    private readonly string _connectionString;
    private readonly ConcurrentDictionary<string, ConfigurationItem> _cache = new();
    private readonly IConnection _rabbitConnection;
    private readonly IModel _channel;
    private readonly Timer _refreshTimer;

    public ConfigurationReader(string applicationName, string connectionString, string rabbitMqHost, int refreshTimerIntervalInMs)
    {
        _applicationName = applicationName;
        _connectionString = connectionString;

        LoadAllFromDb(); // Başlangıç yüklemesi

        // RabbitMQ subscriber
        var factory = new ConnectionFactory() { HostName = rabbitMqHost };
        _rabbitConnection = factory.CreateConnection();
        _channel = _rabbitConnection.CreateModel();
        _channel.ExchangeDeclare("config.events", ExchangeType.Direct, durable: true);

        var queueName = $"config.{_applicationName}";
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, "config.events", _applicationName);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var ev = JsonSerializer.Deserialize<ConfigChangedEvent>(json);
            if (ev != null) ApplyEvent(ev);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        // Timer ile periyodik refresh
        _refreshTimer = new Timer(refreshTimerIntervalInMs);
        _refreshTimer.Elapsed += (s, e) => LoadAllFromDb();
        _refreshTimer.AutoReset = true;
        _refreshTimer.Start();
    }

    private void LoadAllFromDb()
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Name, Type, Value, IsActive, UpdatedAt 
                                FROM ConfigurationRecords 
                                WHERE ApplicationName = @app";
            cmd.Parameters.AddWithValue("@app", _applicationName);

            var dbRecords = new List<ConfigurationItem>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dbRecords.Add(new ConfigurationItem
                {
                    Name = reader.GetString(0),
                    Type = reader.GetString(1),
                    Value = reader.GetString(2),
                    IsActive = reader.GetBoolean(3),
                    UpdatedAt = reader.GetDateTime(4)
                });
            }

            foreach (var record in dbRecords)
            {
                if (record.IsActive)
                {
                    _cache.AddOrUpdate(record.Name, record, (k, old) =>
                        old.UpdatedAt < record.UpdatedAt ? record : old);
                }
                else _cache.TryRemove(record.Name, out _);
            }

            var dbNames = dbRecords.Select(x => x.Name).ToHashSet();
            foreach (var key in _cache.Keys)
                if (!dbNames.Contains(key)) _cache.TryRemove(key, out _);
        }
        catch { /* DB yoksa eski cache kullan */ }
    }

    private void ApplyEvent(ConfigChangedEvent ev)
    {
        if (ev.ApplicationName != _applicationName) return;
        switch (ev.ChangeType)
        {
            case "Added":
            case "Updated":
                if (ev.IsActive)
                    _cache[ev.Name] = new ConfigurationItem
                    {
                        Name = ev.Name,
                        Type = ev.Type!,
                        Value = ev.Value!,
                        IsActive = ev.IsActive,
                        UpdatedAt = ev.UpdatedAt
                    };
                else _cache.TryRemove(ev.Name, out _);
                break;
            case "Deleted":
                _cache.TryRemove(ev.Name, out _);
                break;
        }
    }

    public T GetValue<T>(string key)
    {
        if (!_cache.TryGetValue(key, out var item))
            throw new KeyNotFoundException($"'{key}' not found for {_applicationName}.");
        return (T)Convert.ChangeType(item.Value, typeof(T));
    }

    public void Dispose()
    {
        _refreshTimer?.Stop();
        _channel?.Dispose();
        _rabbitConnection?.Dispose();
    }
}