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
    public List<Tags> Tags { get; set; }
    public ICollection<LinkVisit> LinkVisits { get; set; }
    public ICollection<Reminder> Reminders { get; set; }

    public void Enable()
        => IsDisabled = false;

    public void Disable()
        => IsDisabled = true;
}

public class Tags
{
    public string Title { get; set; }
    public byte Order { get; set; }
}
