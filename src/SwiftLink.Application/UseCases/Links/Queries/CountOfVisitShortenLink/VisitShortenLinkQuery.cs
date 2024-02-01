using MediatR;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortenLink;

public record CountVisitShortenLinkQuery : IRequest<Result<string>>
{
    public int LinkId { get; set; }
}