using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SwiftLink.Infrastructure.Persistence.Config;

public class LinkVisitConfig : IEntityTypeConfiguration<LinkVisit>
{
    public void Configure(EntityTypeBuilder<LinkVisit> builder)
    {
        builder.ToTable(t =>
            t.HasComment("analytics, providing insights into the number of users who clicked on a shortened link."));

        builder.HasKey(t => t.Id)
            .HasName("PK_Base_LinkVisit");

        builder.Property(x => x.ClientMetaData)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Date)
            .IsRequired();
    }
}