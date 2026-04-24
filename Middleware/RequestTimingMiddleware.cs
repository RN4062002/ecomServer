using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ecomServer.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();               // Start timing the request
            await _next(httpContext);
            watch.Stop();

            Console.WriteLine($"Request took {watch.ElapsedMilliseconds} ms"); // Log the time taken to process the request
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimingMiddleware>();
        }
    }
}
