using DynamicConfiguration.Shared.ConfigReader.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicConfiguration.Shared.ConfigReader.Data;

public class AppDbContext : DbContext
{
    public DbSet<ConfigurationRecord> ConfigurationRecords { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigurationRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Type).HasMaxLength(50).IsRequired();
            e.Property(x => x.Value).IsRequired();
            e.Property(x => x.ApplicationName).HasMaxLength(200).IsRequired();
            e.HasIndex(x => new { x.ApplicationName, x.Name });

        });
    }
}