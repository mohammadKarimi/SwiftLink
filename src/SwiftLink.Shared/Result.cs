namespace SwiftLink.Shared;

public class Result
{
    public bool IsSuccess { get; init; }

    public bool IsFailure
        => !IsSuccess;

    public Error Error { get; init; }

    public Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }

    public Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Failure(Error error)
        => new(error);

    public static Result Success()
        => new();

    public static Result<T> Failure<T>(Error error)
        => Result<T>.Failure(error);

    public static Result<T> Success<T>(T data)
        => Result<T>.Success(data);
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(T data, Error error) : base(error)
         => Data = data;

    public static Result<T> Success(T result)
       => new(result, Error.None);

    public new static Result<T> Failure(Error error)
        => new(default!, error);
}
