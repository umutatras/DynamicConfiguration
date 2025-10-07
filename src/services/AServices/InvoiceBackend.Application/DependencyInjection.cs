using FluentValidation;
using InvoiceBackend.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InvoiceBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicaton(this IServiceCollection services)
    {
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviours<,>));
            });
            return services;
        }
    }
}

