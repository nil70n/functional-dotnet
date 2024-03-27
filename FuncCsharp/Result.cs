using System.Diagnostics.CodeAnalysis;

public class Result
{
    public bool Success { get; private set; }
    public string Error { get; private set; }

    protected Result(bool success, string error)
    {
        Success = success;
        Error = error;
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(default(T), false, message);
    }

    public static Result Ok()
    {
        return new Result(true, String.Empty);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, String.Empty);
    }
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    protected internal Result([AllowNull] T value, bool success, string error)
        : base(success, error)
    {
        Value = value;
    }
}
