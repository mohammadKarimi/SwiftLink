namespace SwiftLink.Shared;
public sealed record Error(string Code, string? Message = null)
{

    public static readonly Error None = new(string.Empty, string.Empty);
    public static implicit operator Result(Error error) => Result.Failure(error);
}
