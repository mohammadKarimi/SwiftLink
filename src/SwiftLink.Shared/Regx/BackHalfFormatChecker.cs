using System.Text.RegularExpressions;

namespace SwiftLink.Shared;

public static partial class BackHalfFormatChecker
{
    private const string Pattern = @"^[a-zA-Z0-9]*$";

    [GeneratedRegex(Pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex WordRegex();
}
