using asp.net_jwt.Data;
using asp.net_jwt.Data.Response;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace asp.net_jwt.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware or endpoint
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Handle any exceptions thrown
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Default values for status code and message
            var statusCode = HttpStatusCode.InternalServerError; // Default to 500
            var message = "An unexpected error occurred.";

            // Set the response properties
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Create the JSON response
            var response = new ApiResponse
            {
                StatusCode = (int)statusCode,
                Message = message,
                Details = exception.Message // Full exception message for debugging
            };

            // Write the response as JSON
            var responseJson = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(responseJson);
        }
    }
}
