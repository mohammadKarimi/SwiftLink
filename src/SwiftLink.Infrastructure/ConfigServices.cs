using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SwiftLink.Application.Services.ExpirationNotifiers;
using SwiftLink.Infrastructure.CacheProvider;
using SwiftLink.Infrastructure.JobQuartz;
using SwiftLink.Infrastructure.JobQuartz.Jobs;
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

    public static IServiceCollection AddNotifierServices(this IServiceCollection services)
    {
        services.AddSingleton<IExpirationNotifierComponent, EmailNotifier>();
        return services;
    }

    public static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var jobConfigs = configuration.GetSection(nameof(JobConfigurations)).Get<JobConfigurations>();

        if (jobConfigs == null || jobConfigs.Configurations == null) return services;

        var notifierJob = jobConfigs.Configurations.FirstOrDefault(p =>p.IsEnabled && p.Name == nameof(ExpirationNotifierJob));

        if (notifierJob!=null)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey(nameof(ExpirationNotifierJob));
                q.AddJob<ExpirationNotifierJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{nameof(ExpirationNotifierJob)}-trigger")
                    .StartAt(DateTimeOffset.UtcNow.AddMinutes(notifierJob.StartDelay.TotalMinutes))
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes((int)notifierJob.Interval.TotalMinutes)
                        .RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        return services;
    }
}
