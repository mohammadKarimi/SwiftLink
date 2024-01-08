namespace SwiftLink.Domain.Entities;

/// <summary>
/// this class is aimed for analytics and viewing how many users clicked on a shorter link.
/// </summary>
[Entity]
public class LinkCounter
{
    public long Id { get; set; }

    public int LinkId { get; set; }
    public Link Link { get; set; }

    public DateTime Date { get; set; }
}