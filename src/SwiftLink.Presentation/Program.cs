using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using SwiftLink.Application;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Infrastructure;
using SwiftLink.Infrastructure.Persistence.Context;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Presentation.Filters;
using SwiftLink.Presentation.Middleware;
using SwiftLink.Presentation.Services;
using SwiftLink.Shared;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPolicyRegistry()
                    .AddPolicies();

    builder.Services.AddScoped<IUser, CurrentUser>();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddOptions<AppSettings>()
        .Bind(builder.Configuration.GetSection(AppSettings.ConfigurationSectionName))
        .ValidateDataAnnotations();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.RegisterApplicationServices(builder.Configuration)
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

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddMetricServer(options =>
    {
        options.Port = ushort.Parse(builder.Configuration["AppSettings:DefaultPrometheusPort"]);
    });

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "SwiftLink",
            Version = "v1",
        });

        options.OperationFilter<SwaggerAuthenticationFilter>();
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
        await app.InitializeDatabaseAsync();

    app.UseExceptionHandler();
    app.UseAuthorization();
    app.MapControllers();

    app.UseRouting().UseEndpoints(config =>
    {
        config.MapHealthChecks("/health/check", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    });

    app.UseMetricServer();
    app.UseHttpMetrics(options =>
    {
        options.ReduceStatusCodeCardinality();
        options.AddCustomLabel("Host_IP", context => context.Request.Host.Host);
    });
    app.MapMetrics();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SwiftLink API v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseSerilogRequestLogging();

    //app.UseElasticApm(builder.Configuration,
    //    new HttpDiagnosticsSubscriber(),
    //    new EfCoreDiagnosticsSubscriber());

    app.Run();
}
