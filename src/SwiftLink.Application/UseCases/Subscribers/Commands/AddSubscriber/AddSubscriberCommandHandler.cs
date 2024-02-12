using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UseCases.Subscribers.Commands;
public class AddSubscriberCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<AddSubscriberCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<Guid>> Handle(AddSubscriberCommand request, CancellationToken cancellationToken)
    {
        Subscriber subscriber = new()
        {
            Email = request.Email,
            IsActive = true,
            Name = request.Name,
            Token = Guid.NewGuid()
        };
        _dbContext.Set<Subscriber>().Add(subscriber);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return result.IsFailure ? Result.Failure<Guid>(CommonMessages.Database.InsertFailed) : Result.Success(subscriber.Token);
    }
}
