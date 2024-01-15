using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SwiftLink.Application.Behaviors;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using System.Reflection;

namespace SwiftLink.Application;

public static class ConfigServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddScoped<IShortCodeGenerator, HashBasedShortCodeGenerator>();
        services.AddScoped<ISharedContext, SharedContext>();
        return services;
    }
}