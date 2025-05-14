namespace Shared.Result;

public class ResultBase(int statusCode, bool isSuccess, string message ) : IResultBase
{
    public bool IsSuccess { get; } = isSuccess;
    public string Message { get; } = message;
    public int StatusCode { get; } = statusCode;
    public object? Data { get; } = null;
}

public class ResultBase<TResponse>(TResponse? data = default, int statusCode = 200, bool isSuccess = true, string message = "")
    : IResultBase
{
    public bool IsSuccess { get; } = isSuccess;
    public string Message { get; } = message;
    public int StatusCode { get; } = statusCode;
    public object? Data { get; } = data;
}