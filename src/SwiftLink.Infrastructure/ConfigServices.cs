using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwiftLink.Infrastructure.CacheProvider;
using SwiftLink.Infrastructure.Persistence.Context;

namespace SwiftLink.Infrastructure;

/// <summary>
/// This extension is programmed for registering Infrastructure services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(ApplicationDbContext));
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString,
                (db) => { db.MigrationsHistoryTable("MigrationHistory"); })
#if DEBUG
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddSingleton<ICacheProvider, RedisCacheService>();
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["AppSettings:Redis:RedisCacheUrl"];
        });

        return services;
    }
}
