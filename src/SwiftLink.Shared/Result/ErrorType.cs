using System.ComponentModel;

namespace SwiftLink.Shared;
public enum ErrorType
{
    [Description("Bad Request")]
    Validation = 400,

    [Description("Internal Server Error")]
    Failure = 500,

    [Description("Not Found")]
    NotFound = 404,

    [Description("No Warning and Error")]
    None = 100
}
