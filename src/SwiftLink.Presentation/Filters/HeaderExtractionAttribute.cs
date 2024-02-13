using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace SwiftLink.Presentation.Filters;

public class HeaderExtractionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
        => context.HttpContext.Items.Add("ClientMetaData", JsonSerializer.Serialize(new
        {
            OperationSystem = context.HttpContext.Request.Headers["sec-ch-ua-platform"],
            Mobile = context.HttpContext.Request.Headers["sec-ch-ua-mobile"],
            context.HttpContext.Request.Headers.UserAgent,
            Browser = context.HttpContext.Request.Headers["sec-ch-ua"]
        }));
}
