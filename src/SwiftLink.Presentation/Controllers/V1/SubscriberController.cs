namespace SwiftLink.Presentation.Controllers.V1;

public class SubscriberController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Add(AddSubscriberCommand command, CancellationToken cancellationToken)
          => OK(await MediatR.Send(command, cancellationToken));
}
