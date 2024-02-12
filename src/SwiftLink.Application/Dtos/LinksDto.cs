namespace SwiftLink.Application.Dtos;
public record struct LinksDto
{
    public int LinkdId { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsBanned { get; set; }
}

public record CountOfVisitLinkDto
{
    public int Count { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
}
