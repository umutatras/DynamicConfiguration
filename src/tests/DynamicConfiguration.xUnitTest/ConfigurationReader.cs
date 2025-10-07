using DynamicConfiguration.Shared.ConfigReader;
using DynamicConfiguration.Shared.ConfigReader.Models;
using DynamicConfiguration.Shared.ConfigReader.Events;
using System.Collections.Concurrent;
using System.Reflection;

namespace DynamicConfiguration.xUnitTest;

// xUnit test sýnýfý, IDisposable ile kaynaklarý temizler
public class ConfigurationReaderTests : IDisposable
{
    // Test edilecek ConfigurationReader nesnesi
    private readonly ConfigurationReader _reader;

    // Test için kullanýlacak sabitler
    private readonly string _applicationName = "TestApp";
    private readonly string _connectionString = "Server=localhost;Database=master;User Id=sa;Password=Your_password123;";
    private readonly string _rabbitMqHost = "localhost";
    private readonly int _refreshInterval = 60000; // 60 saniye

    // Kurucu metot, test nesnesini oluþturur ve önceden cache verisi ekler
    public ConfigurationReaderTests()
    {
        // ConfigurationReader nesnesi oluþturuluyor
        _reader = new ConfigurationReader(_applicationName, _connectionString, _rabbitMqHost, _refreshInterval);

        // _cache alaný private olduðu için reflection ile eriþiyoruz
        var cacheField = typeof(ConfigurationReader).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);

        // Test verisi olarak ConcurrentDictionary oluþturuluyor
        var cache = new ConcurrentDictionary<string, ConfigurationItem>();
        cache["TestKey"] = new ConfigurationItem
        {
            Name = "TestKey",
            Type = "string",
            Value = "TestValue",
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        // Test cache'i ConfigurationReader nesnesine set ediliyor
        cacheField.SetValue(_reader, cache);
    }

    // Test: GetValue doðru deðeri döndürmeli
    [Fact]
    public void GetValue_ReturnsCorrectValue()
    {
        var value = _reader.GetValue<string>("TestKey");
        Assert.Equal("TestValue", value); // Beklenen deðer TestValue
    }

    // Test: Eksik key için KeyNotFoundException fýrlatmalý
    [Fact]
    public void GetValue_ThrowsKeyNotFoundException_WhenKeyMissing()
    {
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("MissingKey"));
    }

    // Test: Dispose metodu timer ve kaynaklarý düzgün kapatmalý
    [Fact]
    public void Dispose_StopsTimerAndDisposesResources()
    {
        // Dispose çaðrýsý herhangi bir hata fýrlatmamalý
        _reader.Dispose();
    }

    // Test: Yeni veya güncellenmiþ aktif event cache'e eklenmeli veya güncellenmeli
    [Fact]
    public void ApplyEvent_AddedOrUpdated_Active_AddsOrUpdatesCache()
    {
        var ev = new ConfigChangedEvent
        {
            ApplicationName = _applicationName,
            ChangeType = "Added",
            Name = "NewKey",
            Type = "string",
            Value = "NewValue",
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        // Private ApplyEvent metodunu reflection ile çaðýrýyoruz
        var method = typeof(ConfigurationReader).GetMethod("ApplyEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(_reader, new object[] { ev });

        var value = _reader.GetValue<string>("NewKey");
        Assert.Equal("NewValue", value); // Yeni deðer cache'de olmalý
    }

    // Test: Güncellenmiþ fakat inaktif event cache'den silinmeli
    [Fact]
    public void ApplyEvent_Updated_Inactive_RemovesFromCache()
    {
        var ev = new ConfigChangedEvent
        {
            ApplicationName = _applicationName,
            ChangeType = "Updated",
            Name = "TestKey",
            Type = "string",
            Value = "TestValue",
            IsActive = false,
            UpdatedAt = DateTime.UtcNow
        };

        var method = typeof(ConfigurationReader).GetMethod("ApplyEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(_reader, new object[] { ev });

        // Artýk cache'de olmamalý
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("TestKey"));
    }

    // Test: Silinmiþ event cache'den kaldýrýlmalý
    [Fact]
    public void ApplyEvent_Deleted_RemovesFromCache()
    {
        var ev = new ConfigChangedEvent
        {
            ApplicationName = _applicationName,
            ChangeType = "Deleted",
            Name = "TestKey",
            Type = "string",
            Value = "TestValue",
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        var method = typeof(ConfigurationReader).GetMethod("ApplyEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(_reader, new object[] { ev });

        // Cache'de olmamalý
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("TestKey"));
    }

    // IDisposable implementasyonu: kaynaklarý temizle
    public void Dispose()
    {
        _reader.Dispose();
    }
}
