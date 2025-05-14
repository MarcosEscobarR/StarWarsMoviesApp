using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this IResultBase result)
    {
        var payload = new
        {
            result.StatusCode,
            result.Message,
            result.Data
        };
        
        if (result.IsSuccess)
        {
            return new OkObjectResult(payload);
        }
        
        return result.StatusCode switch
        {
            400 => new BadRequestObjectResult(payload),
            404 => new NotFoundObjectResult(payload),
            _ => new ObjectResult(payload) { StatusCode = result.StatusCode }
        };
    }
}