using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Logging;
using SwiftLink.Domain.Common;
using SwiftLink.Infrastructure.Persistence.Consts;
using SwiftLink.Infrastructure.Persistence.Extensions;

namespace SwiftLink.Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
    : DbContext(options), IApplicationDbContext
{
    private readonly ILogger<ApplicationDbContext> _logger = logger;
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(200);
        configurationBuilder.Properties<string>().AreUnicode(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(EntitySchema.BASE);
        modelBuilder.RegisterEntities(typeof(EntityAttribute).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
    }

    public new async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken) is 0
                ? ConstantMessages.SaveChangesFailed
                : Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DataBase_Exception");
            return ConstantMessages.SaveChangesFailed;
        }
    }
}
