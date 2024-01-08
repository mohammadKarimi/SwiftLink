using System.Text.Json.Serialization;

namespace SwiftLink.Shared;

public class Result
{
    public bool IsSuccess { get; init; }

    [JsonIgnore]
    public bool IsFailure => !IsSuccess;

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
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(T data, string message) : base(message)
         => Data = data;

    public static Result<T> Success(T result, string message = Error.DefaultMessage)
        => new(result, message);

    public new static Result<T> Failure(string errorMessage = Error.DefaultMessage)
        => new(default!, errorMessage);
}
