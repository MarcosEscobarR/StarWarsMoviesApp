namespace Shared.Result;

public interface IResultBase
{
    internal bool IsSuccess { get; }
    internal string Message { get; }
    internal int StatusCode { get; }
    internal object? Data { get; }
}