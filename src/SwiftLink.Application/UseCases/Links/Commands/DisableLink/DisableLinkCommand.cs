namespace SwiftLink.Application.UseCases.Links.Commands;

public record DisableLinkCommand(int Id) : IRequest<Result<bool>>;
