namespace Shared.Result;

public interface IResultBase
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public int StatusCode { get; }
    public object? Data { get; }
}