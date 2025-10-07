using DynamicConfiguration.Shared.ConfigReader;
using DynamicConfiguration.Shared.ConfigReader.Models;
using DynamicConfiguration.Shared.ConfigReader.Events;
using System.Collections.Concurrent;
using System.Reflection;

namespace DynamicConfiguration.xUnitTest;

// xUnit test s�n�f�, IDisposable ile kaynaklar� temizler
public class ConfigurationReaderTests : IDisposable
{
    // Test edilecek ConfigurationReader nesnesi
    private readonly ConfigurationReader _reader;

    // Test i�in kullan�lacak sabitler
    private readonly string _applicationName = "TestApp";
    private readonly string _connectionString = "Server=localhost;Database=master;User Id=sa;Password=Your_password123;";
    private readonly string _rabbitMqHost = "localhost";
    private readonly int _refreshInterval = 60000; // 60 saniye

    // Kurucu metot, test nesnesini olu�turur ve �nceden cache verisi ekler
    public ConfigurationReaderTests()
    {
        // ConfigurationReader nesnesi olu�turuluyor
        _reader = new ConfigurationReader(_applicationName, _connectionString, _rabbitMqHost, _refreshInterval);

        // _cache alan� private oldu�u i�in reflection ile eri�iyoruz
        var cacheField = typeof(ConfigurationReader).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);

        // Test verisi olarak ConcurrentDictionary olu�turuluyor
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

    // Test: GetValue do�ru de�eri d�nd�rmeli
    [Fact]
    public void GetValue_ReturnsCorrectValue()
    {
        var value = _reader.GetValue<string>("TestKey");
        Assert.Equal("TestValue", value); // Beklenen de�er TestValue
    }

    // Test: Eksik key i�in KeyNotFoundException f�rlatmal�
    [Fact]
    public void GetValue_ThrowsKeyNotFoundException_WhenKeyMissing()
    {
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("MissingKey"));
    }

    // Test: Dispose metodu timer ve kaynaklar� d�zg�n kapatmal�
    [Fact]
    public void Dispose_StopsTimerAndDisposesResources()
    {
        // Dispose �a�r�s� herhangi bir hata f�rlatmamal�
        _reader.Dispose();
    }

    // Test: Yeni veya g�ncellenmi� aktif event cache'e eklenmeli veya g�ncellenmeli
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

        // Private ApplyEvent metodunu reflection ile �a��r�yoruz
        var method = typeof(ConfigurationReader).GetMethod("ApplyEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(_reader, new object[] { ev });

        var value = _reader.GetValue<string>("NewKey");
        Assert.Equal("NewValue", value); // Yeni de�er cache'de olmal�
    }

    // Test: G�ncellenmi� fakat inaktif event cache'den silinmeli
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

        // Art�k cache'de olmamal�
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("TestKey"));
    }

    // Test: Silinmi� event cache'den kald�r�lmal�
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

        // Cache'de olmamal�
        Assert.Throws<KeyNotFoundException>(() => _reader.GetValue<string>("TestKey"));
    }

    // IDisposable implementasyonu: kaynaklar� temizle
    public void Dispose()
    {
        _reader.Dispose();
    }
}
