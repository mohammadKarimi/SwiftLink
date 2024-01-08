namespace SwiftLink.Domain.Entities;

/// <summary>
/// This class is designed to store the original URL for each subscriber along with the link code and expiration time.
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