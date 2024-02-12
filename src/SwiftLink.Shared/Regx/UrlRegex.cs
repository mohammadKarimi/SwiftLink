using System.Text.RegularExpressions;

namespace SwiftLink.Shared;

public static partial class UrlFormatChecker
{
    private const string Pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";

    [GeneratedRegex(Pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex UrlRegex();
}
