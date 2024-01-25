using System.Text.RegularExpressions;

namespace SwiftLink.Shared;

public static partial class UrlFormatChecker
{
    private const string _pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex UrlRegex();
}