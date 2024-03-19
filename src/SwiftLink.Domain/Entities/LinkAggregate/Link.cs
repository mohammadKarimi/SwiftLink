namespace SwiftLink.Domain.Entities;

/// <summary>
/// This class is designed to store the original URL for each subscriber along with the link code and expiration time.
/// </summary>
[Entity]
public class Link : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int SubscriberId { get; set; }
    public Subscriber Subscriber { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisabled { get; private set; }
    public string Password { get; set; }
    public string GroupName { get; set; }
    public ICollection<Tag> Tags { get; private set; } = [];
    public ICollection<LinkVisit> LinkVisits { get; set; }
    public ICollection<Reminder> Reminders { get; private set; } = [];

    public void Enable()
        => IsDisabled = false;

    public void Disable()
        => IsDisabled = true;

    public void AddTags(IReadOnlyList<Tag> tags)
    {
        foreach (var item in tags)
            Tags.Add(item);
    }

    public void AddReminder(DateTime reminderDate)
        => Reminders.Add(new Reminder
        {
            Link = this,
            RemindDate = reminderDate,
            TryCount = 0,
        });
}
