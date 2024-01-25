using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using SwiftLink.Infrastructure.CacheProvider;
using SwiftLink.Infrastructure.Persistence.Context;

namespace SwiftLink.Infrastructure;

/// <summary>
/// This extention is programmed for registering Infrastructure services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)),
                (db) => { db.MigrationsHistoryTable("MigrationHistory"); });
        });

        services.AddSingleton<ICacheProvider, RedisCacheService>();
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["AppSettings:Redis:RedisCacheUrl"];
        });


        return services;
    }
}