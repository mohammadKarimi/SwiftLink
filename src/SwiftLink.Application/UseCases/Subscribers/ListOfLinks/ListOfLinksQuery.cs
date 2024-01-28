using MediatR;
using SwiftLink.Application.Common;

namespace SwiftLink.Application.UseCases.Subscribers.ListOfLinks;

public record struct ListOfLinksDto
{
    public int LinkdId { get; set; }
    public DateTime ExpirationTime { get; set; }
}
public record ListOfLinksQuery : IAuthorizedRequest, IRequest<IReadOnlyList<ListOfLinksDto>>
{
    public string Token { get; set; }

}
