using MediatR;

namespace SwiftLink.Application.UseCases.Subscribers.Queries.ListOfLinks;

public record struct ListOfLinksDto
{
    public int LinkdId { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsBanned { get; set; }
}
public record ListOfLinksQuery : IRequest<Result<IReadOnlyList<ListOfLinksDto>>>
{
    public int Count { get; set; }
}
