using DynamicConfiguration.Shared.ConfigReader;
using DynamicConfiguration.Shared.ConfigReader.Data;
using DynamicConfiguration.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonServiceExt(builder.Configuration);
// Add services to the container.

var app = builder.Build();

// ConfigurationReader singleton olarak ekle
var reader = new ConfigurationReader(
    applicationName: "SERVICE-B",
    connectionString: "Server=UMUT;Database=DynamicConfigurationDB;integrated security=true;TrustServerCertificate=True",
    rabbitMqHost: "localhost",
    refreshTimerIntervalInMs: 10000 // 10 saniye
);

// GET /config/{key} endpoint
app.MapGet("/config/{key}", (string key) =>
{
    try
    {
        var value = reader.GetValue<string>(key);
        return Results.Ok(new { Key = key, Value = value });
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { Key = key, Message = "Key bulunamadý" });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.UseHttpsRedirection();



app.Run();
