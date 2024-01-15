using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using SwiftLink.Application;
using SwiftLink.Infrastructure;
using SwiftLink.Infrastructure.Persistence.Context;
using SwiftLink.Presentation.Middleware;
using SwiftLink.Shared;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOptions<AppSettings>()
                 .Bind(builder.Configuration.GetSection(AppSettings.ConfigurationSectionName))
                 .ValidateDataAnnotations();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.RegisterApplicationServices()
                    .RegisterInfrastructureServices(builder.Configuration);

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();

    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services
           .AddHealthChecks()
           .AddSqlServer(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)))
           .AddRedis(builder.Configuration["AppSettings:Redis:RedisCacheUrl"]);

    builder.Services
           .AddExceptionHandler<BusinessValidationExceptionHandling>()
           .AddExceptionHandler<GlobalExceptionHandling>();
    builder.Services.AddProblemDetails();

    builder.Services.AddMetricServer(options =>
    {
        options.Port = 6789;
    });
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.UseAuthorization();
    app.MapControllers();
    app.UseRouting()
       .UseEndpoints(config =>
             {
                 config.MapHealthChecks("/health", new HealthCheckOptions
                 {
                     Predicate = _ => true,
                     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                 });
             });

    app.UseHttpMetrics(options =>
    {
        options.ReduceStatusCodeCardinality();
        options.AddCustomLabel("Host_IP", context => context.Request.Host.Host);
    });

    app.Run();
}
