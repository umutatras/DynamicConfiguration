using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class RoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
{
    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.ToTable("RoleClaims");
    }
}