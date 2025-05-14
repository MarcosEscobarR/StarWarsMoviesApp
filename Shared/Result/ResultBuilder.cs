namespace Shared.Result;

public class ResultBuilder
{
    public static ResultBase IsOk(string message = "", int statusCode = 200 )
    {
        return new ResultBase(statusCode, true, message);
    }
    public static ResultBase<T> IsOk<T>(T? data = default, string message = "", int statusCode = 200 )
    {
        return new ResultBase<T>(data, statusCode, true, message);
    }

    public static ResultBase IsFailure(  string message, int statusCode = 400)
    {
        return new ResultBase(statusCode, false, message);
    }

    public static ResultBase<T> IsFailure<T>(  string message, int statusCode = 400)
    {
        return new ResultBase<T>(default, statusCode, false, message);
    }
    public static ResultBase IsNotFound( string message = "", int statusCode = 404)
    {
        return new ResultBase(statusCode, false, message);
    }

    public static ResultBase<T> IsNotFound<T>(string message = "", int statusCode = 404 )
    {
        return new ResultBase<T>(default, statusCode, false, message);
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