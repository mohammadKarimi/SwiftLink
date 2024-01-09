using System.ComponentModel.DataAnnotations;

namespace SwiftLink.Shared;
public record AppSettings
{
    public static string ConfigurationSectionName => "AppSettings";

    [Url]
    public string DefaultUrlOnNotFound { get; set; }

    [Url]
    public string RedisCacheUrl { get; set; }
}
