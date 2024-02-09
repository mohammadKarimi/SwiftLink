using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwiftLink.Application.Behaviors;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using System.Reflection;

namespace SwiftLink.Application;

public static class ConfigServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(SubscriberAuthorizationBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            if (configuration["AppSettings:LoggingBehavior"].ToLower() is "enable")
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IShortCodeGenerator, HashBasedShortCodeGenerator>();
        services.AddScoped<ISharedContext, SharedContext>();
        return services;
    }
}