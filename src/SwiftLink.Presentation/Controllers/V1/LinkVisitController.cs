namespace SwiftLink.Presentation.Controllers.V1;

public class LinkVisitController(ISender sender) : BaseController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetLinkVisitList([FromQuery] GetClientMetaDataByLinkIdQuery query, CancellationToken cancellationToken = default)
        => OK(await MediatR.Send(query, cancellationToken));
}
