using MediatR;
using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Queries;

public record CountVisitShortenLinkQuery : IRequest<Result<CountOfVisitLinkDto>>
{
    public int LinkId { get; set; }
}