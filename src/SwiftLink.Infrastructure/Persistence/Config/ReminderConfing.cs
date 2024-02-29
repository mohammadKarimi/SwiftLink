using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwiftLink.Domain.Entities.Link;

namespace SwiftLink.Infrastructure.Persistence.Config;


public class ReminderConfing : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ToTable(r => r.HasComment("stores information about reminders that will warn about expirationDate of a link"));

        builder.HasKey(r => r.Id)
            .HasName("PK_Base_Reminder");

        builder.Property(r => r.RemindDate)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false);
    }
}
