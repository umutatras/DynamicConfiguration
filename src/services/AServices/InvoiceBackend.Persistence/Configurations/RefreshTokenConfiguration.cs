using InvoiceBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations
{
    public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasIndex(x => x.Token)
                .IsUnique();

            builder.HasIndex(x => new { x.AppUserId, x.Token });

            builder.Property(x => x.Expires)
                .IsRequired();

            builder.Property(x => x.SecurityStamp)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.CreatedOn)
                .IsRequired();

            builder.Property(p => p.CreatedByUserId)
                .IsRequired();

            builder.Property(p => p.ModifiedOn)
                .IsRequired(false);

            builder.Property(p => p.ModifiedByUserId)
                .IsRequired(false);

            builder.ToTable("RefreshTokens");
        }
    }
}
