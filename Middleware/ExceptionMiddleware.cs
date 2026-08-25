using System.Text.Json;
using System.Net;

namespace LifestyleAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate Next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _Next = Next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new { message = "An unexpected error occured.", detail = (string?)ex.Message }
                : new { message = "An unexpected error occured.", detail = (string?)null };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}