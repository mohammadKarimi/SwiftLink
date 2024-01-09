using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SwiftLink.Infrastructure.Persistence.Config;

public class LinkConfig : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable(t => t.HasComment("Stores Original links and generated shortCode."));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_Link");

        builder.Property(x => x.OriginalUrl)
               .IsRequired()
               .HasMaxLength(1500);

        builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(x => x.Password)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(x => x.ShortCode)
               .IsRequired()
               .HasMaxLength(16);

        builder.HasMany(x => x.LinkVisits)
               .WithOne(x => x.Link)
               .HasPrincipalKey(x => x.Id)
               .HasForeignKey(x => x.LinkId);
    }
}
