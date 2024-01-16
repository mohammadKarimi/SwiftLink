using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SwiftLink.Presentation.Controllers;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
[ApiController]
public abstract class BaseController(ISender sender) : Controller
{
    protected readonly ISender MediatR = sender;
}