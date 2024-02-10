using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SwiftLink.Infrastructure.Persistence.Config;

public class SubscriberConfig : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable(t => t.HasComment("Only these subscribers are allowed to insert a URL to obtain a shorter one."));

        builder.HasKey(t => t.Id)
            .HasName("PK_Base_Subscriber");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(75);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Token)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
