using MediatR;

namespace SwiftLink.Application.UseCases.Links.Commands;

public record UpdateLinkCommand:IRequest<Result<bool>>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDisabled { get; set; }
}
