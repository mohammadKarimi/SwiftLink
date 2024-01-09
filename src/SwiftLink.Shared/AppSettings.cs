using System.ComponentModel.DataAnnotations;

namespace SwiftLink.Shared;
public record AppSettings
{
    public static string ConfigurationSectionName => "AppSettings";

    [Range(1, 60)]
    public byte DefaultExpirationTimeInDays { get; set; }

    [Url]
    public string DefaultUrlOnNotFound { get; set; }

    [Url]
    public string RedisCacheUrl { get; set; }
}
