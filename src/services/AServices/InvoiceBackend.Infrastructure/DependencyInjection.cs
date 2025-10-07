using InvoiceBackend.Application.Interfaces;
using InvoiceBackend.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");


        services.AddScoped<IJwtService, JwtManager>();

        services.AddScoped<IIdentityService, IdentityManager>();
        return services;
    }
}
