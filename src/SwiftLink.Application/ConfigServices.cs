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

            if (configuration["AppSettings:LoggingBehavior"]?.ToLower() is "enable")
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        services.AddTransient<IShortCodeGenerator, HashBasedShortCodeGenerator>();
        services.AddScoped<ISharedContext, SharedContext>();
        return services;
    }
}
