using DynamicConfiguration.Shared.ConfigReader.Data;
using DynamicConfiguration.Shared.ConfigReader.Events;
using DynamicConfiguration.Shared.ConfigReader.Models;
using DynamicConfiguration.Shared.Extensions;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCommonServiceExt(builder.Configuration);

// Add services to the container.

var app = builder.Build();
app.MapGet("/configs/{app}", async (string app, AppDbContext db) =>
{
    return await db.ConfigurationRecords.AsNoTracking().Where(x => x.ApplicationName == app).ToListAsync();
});

app.MapPost("/configs", async (ConfigurationRecord record, AppDbContext db, IModel channel) =>
{
    record.Id = Guid.NewGuid();
    record.UpdatedAt = DateTime.UtcNow;
    db.ConfigurationRecords.Add(record);
    await db.SaveChangesAsync();
    PublishEvent(record, "Added", channel);
    return Results.Ok(record);
});

app.MapPut("/configs/{id:guid}", async (Guid id, ConfigurationRecord record, AppDbContext db, IModel channel) =>
{
    var existing = await db.ConfigurationRecords.FindAsync(id);
    if (existing == null) return Results.NotFound();

    existing.Value = record.Value;
    existing.Type = record.Type;
    existing.IsActive = record.IsActive;
    existing.UpdatedAt = DateTime.UtcNow;
    await db.SaveChangesAsync();
    PublishEvent(existing, "Updated", channel);
    return Results.Ok(existing);
});

app.MapDelete("/configs/{id:guid}", async (Guid id, AppDbContext db, IModel channel) =>
{
    var record = await db.ConfigurationRecords.FindAsync(id);
    if (record == null) return Results.NotFound();

    db.ConfigurationRecords.Remove(record);
    await db.SaveChangesAsync();
    PublishEvent(record, "Deleted", channel);
    return Results.Ok();
});

void PublishEvent(ConfigurationRecord record, string type, IModel channel)
{
    var ev = new ConfigChangedEvent
    {
        ApplicationName = record.ApplicationName,
        ChangeType = type,
        Name = record.Name,
        Type = record.Type,
        Value = record.Value,
        IsActive = record.IsActive,
        UpdatedAt = record.UpdatedAt
    };
    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ev));
    channel.BasicPublish("config.events", record.ApplicationName, null, body);
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();

