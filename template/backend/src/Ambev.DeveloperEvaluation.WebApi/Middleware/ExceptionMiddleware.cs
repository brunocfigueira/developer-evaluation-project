using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly JsonSerializerOptions _jsonOptions;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
    }
}
