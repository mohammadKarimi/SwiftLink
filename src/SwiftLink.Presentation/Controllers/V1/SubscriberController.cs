using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Subscribers.Commands;

namespace SwiftLink.Presentation.Controllers.V1;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class SubscriberController(ISender sender) : BaseController(sender)
{
    public async Task<IActionResult> Add(AddSubscriberCommand command, CancellationToken cancellationToken)
          => OK(await _mediatR.Send(command, cancellationToken));
}
