using System.Text.RegularExpressions;

namespace SwiftLink.Shared;

public static partial class UrlFormatChecker
{
    private const string _pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex UrlRegex();
}


public static partial class EmailFormatChecker
{
    private const string _pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex EmailRegex();
}