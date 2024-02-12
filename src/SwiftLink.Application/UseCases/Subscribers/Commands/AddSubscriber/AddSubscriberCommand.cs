using SwiftLink.Application.Common.Security;

namespace SwiftLink.Application.UseCases.Subscribers.Commands;

public record AddSubscriberCommand(string Email, string Name) : IAnonymousRequest, IRequest<Result<Guid>>;
