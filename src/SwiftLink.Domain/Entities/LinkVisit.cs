namespace SwiftLink.Domain.Entities;

/// <summary>
/// This class is intended for analytics, providing insights into the number of users who clicked on a shortened link.
/// </summary>
[Entity]
public class LinkVisit : IEntity
{
    public long Id { get; set; }

    public int LinkId { get; set; }
    public Link Link { get; set; }

    public DateTime Date { get; set; }

    public string ClientMetaData { get; set; }
}
