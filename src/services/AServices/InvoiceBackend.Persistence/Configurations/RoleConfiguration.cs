using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

        builder.ToTable("Roles");

        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        builder.Property(u => u.Name).HasMaxLength(100);
        builder.Property(u => u.NormalizedName).HasMaxLength(100);


        builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

        builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

        builder.ToTable("Roles");
    }
}