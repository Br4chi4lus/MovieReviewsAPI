using MovieReviewsAPI.Exceptions;
using System.Text.Json;

namespace MovieReviewsAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ErrorHandlingMiddleware()
        {
            
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (ForbidenException ex)
            {
                context.Response.StatusCode = 403;
            }
        }
    }
}
