using Microsoft.AspNetCore.Mvc.Filters;
using SwiftLink.Shared;

namespace SwiftLink.Presentation.Filters;

public partial class ShortenEndpointFilter : ActionFilterAttribute
{
    // private const int _urlArgumentIndex = 0;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
    }

    //private static bool IsValidUrl(string url)
    //    => UrlFormatChecker.UrlRegex().IsMatch(url);
}