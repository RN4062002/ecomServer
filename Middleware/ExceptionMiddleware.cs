using ecomServer.Data;
using ecomServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ecomServer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext,ErrorLogService errorLogService)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                try
                {
                    await errorLogService.LogErrorAsync(ex, httpContext);
                }
                catch (Exception loggingEx)
                {
                    Console.Error.WriteLine("Failed to persist error log.");
                    Console.Error.WriteLine(loggingEx);
                }

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
