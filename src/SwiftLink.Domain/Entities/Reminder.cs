namespace SwiftLink.Domain.Entities;

/// <summary>
/// This class is responsible for storing information about reminders that will warn about expirationDate of a link
/// </summary>
[Entity]
public class Reminder: IEntity
{
    public int Id { get; set; }
    public int LinkId { get; set; }
    public Link Link { get; set; }
    public byte TryCount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime RemindTime { get; set; }
}
