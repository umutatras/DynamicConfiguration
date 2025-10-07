using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RoleId });

        builder.ToTable("UserRoles");
    }
}