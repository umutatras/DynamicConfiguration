using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Domain.Settings;
using InvoiceBackend.Infrastructure.Identity;
using InvoiceBackend.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceBackend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<HizliBİlDbContext>(opt => opt.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, InvoiceBackend.Persistence.UnitOfWork.UnitOfWork>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<HizliBİlDbContext>()
        .AddDefaultTokenProviders();


        return services;
    }
}
