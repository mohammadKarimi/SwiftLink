using MediatR;
using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Subscribers.Queries;

public record ListOfLinksQuery : IRequest<Result<IReadOnlyList<LinksDto>>>
{
    public int Count { get; set; }
}
