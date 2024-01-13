using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SwiftLink.Presentation.Controllers;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
[ApiController]
public abstract class BaseController : Controller
{
    private ISender? _mediatR;
    protected ISender MediatR => _mediatR ??= HttpContext.RequestServices.GetRequiredService<ISender>();

}