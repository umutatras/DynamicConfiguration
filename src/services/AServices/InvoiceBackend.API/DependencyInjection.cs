using DynamicConfiguration.Shared.ConfigReader;
using InvoiceBackend.API.Services;
using InvoiceBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace InvoiceBackend.API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddSingleton(new ConfigurationReader(
    applicationName: "SERVICE-A",
    connectionString: "Server=UMUT;Database=DynamicConfigurationDB;integrated security=true;TrustServerCertificate=True",
    rabbitMqHost: "localhost",
    refreshTimerIntervalInMs: 10000 // 10 saniye
));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyHeader());
        });
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserManager>();
        services.AddTransient<IEnvironmentService, EnvironmentManager>(sp => new EnvironmentManager(environment.WebRootPath));


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

        })
       .AddJwtBearer(options =>
       {
           var secretKey = configuration["JwtSettings:SecretKey"];

           if (string.IsNullOrEmpty(secretKey))
               throw new ArgumentNullException("JwtSettings:SecretKey is not set.");

           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["JwtSettings:Issuer"],
               ValidAudience = configuration["JwtSettings:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
               ClockSkew = TimeSpan.Zero
           };
       });
        services.AddSwaggerGen(setupAction =>
        {

            setupAction.SwaggerDoc(
                "v1",
                new OpenApiInfo()
                {
                    Title = "InvoiceBackend Web API",
                    Version = "1",
                    Description = "Through this API you can access InvoiceBackend App's details",
                    Contact = new OpenApiContact()
                    {
                        Email = "umutatras@gmail.com",
                        Name = "Umut ATRAŞ",
                        Url = new Uri("https://www.linkedin.com/in/umut-atras/")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "© 2025 UA Tüm Hakları Saklıdır",
                        Url = new Uri("https://www.linkedin.com/in/umut-atras/")
                    }
                });

            setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = $"Input your Bearer token in this format - Bearer token to access this API",
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
        });
        return services;
    }
}
