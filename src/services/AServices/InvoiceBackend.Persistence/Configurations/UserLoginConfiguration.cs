using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class UserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
{
    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {

        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.Property(l => l.LoginProvider).HasMaxLength(128);
        builder.Property(l => l.ProviderKey).HasMaxLength(128);

        builder.ToTable("UserLogins");
    }
}