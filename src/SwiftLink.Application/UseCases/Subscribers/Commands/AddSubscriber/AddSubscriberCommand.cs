namespace SwiftLink.Application.UseCases.Subscribers.Commands;

public record AddSubscriberCommand(string Email,string Name) : IRequest<Result<Guid>>;