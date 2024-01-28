using MediatR;

namespace SwiftLink.Application.UseCases.Subscribers.ListOfLinks;

public record struct ListOfLinksDto
{
    public int LinkdId { get; set; }
    public DateTime ExpirationTime { get; set; }
}
public record ListOfLinksQuery : IRequest<IReadOnlyList<ListOfLinksDto>>
{
    public string Token { get; set; }

}
