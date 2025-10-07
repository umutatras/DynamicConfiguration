using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class UserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("UserClaims");
    }
}