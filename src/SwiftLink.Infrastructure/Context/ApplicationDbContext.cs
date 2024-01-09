using SwiftLink.Domain.Common;
using SwiftLink.Infrastructure.Extensions;
using System.Reflection;

namespace SwiftLink.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(200);
        configurationBuilder.Properties<string>().AreUnicode(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Base");
        modelBuilder.RegisterEntities(typeof(EntityAttribute).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
    }

    public async Task<Result> SaveChangesAsync()
        => await SaveChangesAsync(default);

    public new async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken) is 0 ?
                            Result.Failure("Faile On Save into Database.") :
                            Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.InnerException.Message);
        }
    }

}
