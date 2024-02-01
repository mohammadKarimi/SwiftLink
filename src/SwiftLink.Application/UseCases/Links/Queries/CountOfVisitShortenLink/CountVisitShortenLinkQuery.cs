using MediatR;

namespace SwiftLink.Application.UseCases.Links.Queries;

public record CountVisitShortenLinkQuery : IRequest<Result<string>>
{
    public int LinkId { get; set; }
}