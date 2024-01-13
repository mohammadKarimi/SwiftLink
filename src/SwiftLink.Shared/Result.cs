namespace SwiftLink.Shared;

public class Result
{
    public bool IsSuccess { get; init; }

    public bool IsFailure
        => !IsSuccess;

    public string Message { get; init; }

    public Result()
      => IsSuccess = true;

    public Result(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    public static Result Failure(string message)
        => new(message);

    public static Result Success()
        => new();

    public static Result Failure<T>(string message)
        => Result<T>.Failure(message);

    public static Result Success<T>()
        => Success();
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(T data, string message) : base(message)
         => Data = data;

    public static Result<T> Success(T result)
       => new(result, string.Empty);

    public new static Result<T> Failure(string errorMessage)
        => new(default!, errorMessage);
}
