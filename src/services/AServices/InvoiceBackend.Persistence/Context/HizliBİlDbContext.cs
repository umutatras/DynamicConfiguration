using InvoiceBackend.Domain.Entities;
using InvoiceBackend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBackend.Persistence.Context;

public class HizliBİlDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
    public HizliBİlDbContext()
    {

    }
    public HizliBİlDbContext(DbContextOptions<HizliBİlDbContext> options) : base(options)
    {

    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(HizliBİlDbContext).Assembly);
    }
}