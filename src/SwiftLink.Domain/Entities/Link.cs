namespace SwiftLink.Domain.Entities;

/// <summary>
/// This class is aimed for storing Original Url for each subscriber plus Link code and expiration time.
/// </summary>
[Entity]
public class Link
{
    public int Id { get; set; }
    public int SubscriberId { get; set; }
    public Subscriber Subscriber { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public DateTime ExpirationDate { get; set; }
}
