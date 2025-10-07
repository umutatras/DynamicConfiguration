using InvoiceBackend.Domain.Entities;
using InvoiceBackend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistance.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        builder.Property(u => u.UserName).HasMaxLength(100);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(100);

        builder.Property(u => u.Email).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property(u => u.Email).HasMaxLength(100);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(100);

        builder.Property(u => u.PhoneNumber).IsRequired(false);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);




        builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

        builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

        builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

        builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();


        builder.HasMany<RefreshToken>()
          .WithOne()
          .HasForeignKey(x => x.AppUserId);


        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(user => user.CreatedByUserId)
            .IsRequired();

        builder.Property(user => user.ModifiedOn)
            .IsRequired(false);

        builder.Property(user => user.ModifiedByUserId)
            .IsRequired(false);


        builder.ToTable("Users");
    }
}