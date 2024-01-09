using Microsoft.AspNetCore.Mvc.Filters;
using SwiftLink.Application.UseCases.Links.GenerateCommand;
using System.Text.RegularExpressions;

namespace SwiftLink.Presentation.Filters;

public partial class ShortenEndpointFilter : ActionFilterAttribute
{
    private const int _urlArgumentIndex = 0;
    private const string _pattern = @"^(?:(?:https?|ftp)://)?[^\s/$.?#].[^\s]*$";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
    }
 
    private static bool IsValidUrl(string url)
        => ShortCodeRegex().IsMatch(url);

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ShortCodeRegex();
}