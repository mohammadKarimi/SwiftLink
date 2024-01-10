﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.GenerateCommand;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers;

[ApiVersion("1.0")]
public class LinkController : BaseController
{
    [HttpPost]
    [ShortenEndpointFilter]
    public IActionResult Shorten([FromBody] GenerateShortCodeCommand command)
    {
        return Ok(MediatR.Send(command));
    }
}