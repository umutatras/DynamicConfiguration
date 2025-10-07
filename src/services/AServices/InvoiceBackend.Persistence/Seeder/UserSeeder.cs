using InvoiceBackend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceBackend.Persistence.Seeder;

public class UserSeeder : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        var initialUserId = Guid.Parse("2798212b-3e5d-4556-8629-a64eb70da4a8");

        var initialUser = new AppUser
        {
            Id = 1,
            UserName = "umut",
            NormalizedUserName = "UMUT",
            Email = "umut@gmail.com",
            NormalizedEmail = "UMUT@GMAIL.COM",
            EmailConfirmed = true,
            CreatedByUserId = 1,
            CreatedOn = new DateTimeOffset(new DateTime(2025, 03, 11)),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            FirstName = "Umut",
            LastName = "Atraş",
            SecurityStamp = Guid.NewGuid().ToString(),
            LockoutEnabled = false,
            AccessFailedCount = 0,

        };
        initialUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(initialUser, "123umut123");

        builder.HasData(initialUser);
    }
}