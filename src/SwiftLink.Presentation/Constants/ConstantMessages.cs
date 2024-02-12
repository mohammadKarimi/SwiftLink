using SwiftLink.Shared;

namespace SwiftLink.Presentation.Constants;

internal static class ConstantMessages
{
    public static Error UnHandledExceptions(string exception = "")
        => Error.Failure("oops! something went wrong :(", exception);
}
