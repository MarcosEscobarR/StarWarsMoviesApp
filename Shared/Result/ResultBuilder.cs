namespace Shared.Result;

public class ResultBuilder
{
    public static ResultBase IsOk( int statusCode = 200, string message = "")
    {
        return new ResultBase(statusCode, true, message);
    }
    public static ResultBase<T> IsOk<T>(T? data = default, int statusCode = 200, string message = "")
    {
        return new ResultBase<T>(data, statusCode, true, message);
    }

    public static ResultBase IsFailure(int statusCode = 400, string message = "")
    {
        return new ResultBase(statusCode, false, message);
    }
    
    public static ResultBase<T> IsFailure<T>(T data = default, int statusCode = 400, string message = "")
    {
        return new ResultBase<T>(data, statusCode, false, message);
    }
    
    public static ResultBase IsNotFound( int statusCode = 404, string message = "")
    {
        return new ResultBase(statusCode, false, message);
    }
    
    public static ResultBase<T> IsNotFound<T>(T data, int statusCode = 404, string message = "")
    {
        return new ResultBase<T>(data, statusCode, false, message);
    }
    
    public static ResultBase StatusCode(int statusCode, string message = "")
    {
        return new ResultBase(statusCode, false, message);
    }
    
    public static ResultBase<T> StatusCode<T>(T data, int statusCode, string message = "")
    {
        return new ResultBase<T>( data,statusCode, false, message);
    }
}