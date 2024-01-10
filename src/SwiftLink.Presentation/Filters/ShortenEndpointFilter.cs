using Microsoft.AspNetCore.Mvc.Filters;
using SwiftLink.Application.UseCases.Links.GenerateCommand;
using SwiftLink.Shared;
using System.Text.RegularExpressions;

namespace SwiftLink.Presentation.Filters;

public partial class ShortenEndpointFilter : ActionFilterAttribute
{
    private const int _urlArgumentIndex = 0;
   
    public override void OnActionExecuting(ActionExecutingContext context)
    {
    }
 
    private static bool IsValidUrl(string url)
        => UrlFormatChecker.UrlRegex().IsMatch(url);
 
}