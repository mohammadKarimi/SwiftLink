using System;
using Ardalis.GuardClauses;
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

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString,
                (db) => { db.MigrationsHistoryTable("MigrationHistory"); });
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
