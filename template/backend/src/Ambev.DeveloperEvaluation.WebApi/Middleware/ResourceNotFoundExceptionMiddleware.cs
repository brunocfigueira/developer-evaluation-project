
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ResourceNotFoundExceptionMiddleware : ExceptionMiddleware
    {

        public ResourceNotFoundExceptionMiddleware(RequestDelegate next): base(next)
        {

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }           
            catch (ResourceNotFoundException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, KeyNotFoundException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = new 
            {
                Type = "ResourceNotFound",
                Error = "Resource Not Found",
                Detail = exception.Message            
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }
    }
}
