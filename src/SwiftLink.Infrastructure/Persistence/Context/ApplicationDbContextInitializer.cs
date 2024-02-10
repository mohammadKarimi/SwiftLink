using Microsoft.Extensions.Logging;

namespace SwiftLink.Infrastructure.Persistence.Context;

public class ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default Subscriber
        if (!_context.Set<Subscriber>().Any())
        {
            Subscriber subscriber = new()
            {
                Email = "Default@localhost",
                IsActive = true,
                Name = "Default Subscriber",
                Token = new Guid("5F9E9F0D-2413-4E4B-8E38-9EEBE9503E52")
            };

            _context.Set<Subscriber>().Add(subscriber);

            await _context.SaveChangesAsync();
        }
    }
}
