using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SwiftLink.Application;
using SwiftLink.Infrastructure;
using SwiftLink.Infrastructure.Persistence.Context;
using SwiftLink.Presentation;
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
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services
           .AddHealthChecks()
           .AddSqlServer(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)))
           .AddRedis(builder.Configuration["AppSettings:Redis:RedisCacheUrl"]);
}

var app = builder.Build();
{
    app.UseExceptionHandler(error =>
    {
        error.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            await context.Response.WriteAsync(Result.Failure(Constants.UnHandledExceptions).ToString()!);
        });
    });

    // app.UseHttpsRedirection();
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
    app.Run();
}