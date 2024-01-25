using System.Text.Json.Serialization;

namespace SwiftLink.Shared;

public class Result
{
    public bool IsSuccess { get; init; }

    [JsonIgnore] public bool IsFailure => !IsSuccess;

    public Error Error { get; init; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(Error error) => new(false, error);

    public static Result Success() => new(true, Error.None);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(bool isSuccess, T data, Error error) : base(isSuccess, error)
    {
        Data = data;
    }

    public static Result<T> Success(T result) => new(true, result, Error.None);

    public new static Result<T> Failure(Error error) => new(false, default!, error);
}