using SwiftLink.Shared;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddOptions<AppSettings>()
                    .Bind(builder.Configuration.GetSection(AppSettings.ConfigurationSectionName))
                    .ValidateDataAnnotations();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["AppSettings:RedisCacheUrl"];
    });
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
