using SwiftLink.Shared;

namespace SwiftLink.Presentation;

internal static class ConstantMessages
{
    public static Error UnHandledExceptions(string exception = "")
        => Error.Failure("oops! something went wrong :(", exception);
}