
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class BusinessRuleExceptionMiddleware : ExceptionMiddleware
    {
        public BusinessRuleExceptionMiddleware(RequestDelegate next) : base(next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessRuleException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }         
        }

        private Task HandleValidationExceptionAsync(HttpContext context, BusinessRuleException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new
            {
                Type = "BusinessRuleException",
                Error = "Violation of business rules",
                Detail = exception.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }
    }
}