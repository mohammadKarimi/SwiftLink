using MediatR;

namespace SwiftLink.Application.UseCases.Links.Commands;

public record DisableLinkCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
