using MediatR;

namespace SwiftLink.Application.UseCases.Links.Commands.DisableLink;

public class DisableLinkCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
