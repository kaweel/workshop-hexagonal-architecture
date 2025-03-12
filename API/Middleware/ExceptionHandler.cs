using Common.ApiException;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var response = new { message = exception.Message };
        
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            case ForbiddenException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new { message = "Internal Server Error" };
                break;
        }

        await context.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
