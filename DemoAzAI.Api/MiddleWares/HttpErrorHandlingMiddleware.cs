using Azure.Identity;
using System.Text.Json;

namespace DemoAzAI.Api.MiddleWares
{
    public class HttpErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpErrorHandlingMiddleware> _logger;

        public HttpErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<HttpErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext httpContext,
            Exception ex)
        {

            _logger.LogError(
                ex,
                $"Exception: {nameof(HttpErrorHandlingMiddleware)}, {ex.GetType().Name}, {ex.Message}");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize("An error has occured and has been logged"));
        }
    }
}


