using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this ResultBase<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result);
        }

        return result.StatusCode switch
        {
            400 => new BadRequestObjectResult(result),
            404 => new NotFoundObjectResult(result),
            _ => new ObjectResult(result) { StatusCode = result.StatusCode }
        };
    }
    
    public static IActionResult ToActionResult(this ResultBase result)
    {
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                200 => new OkObjectResult(result),
                201 => new CreatedResult(string.Empty, result),
                _ => new ObjectResult(result) { StatusCode = result.StatusCode }
            };
        }

        return result.StatusCode switch
        {
            400 => new BadRequestObjectResult(result),
            404 => new NotFoundObjectResult(result),
            _ => new ObjectResult(result) { StatusCode = result.StatusCode }
        };
    }
}