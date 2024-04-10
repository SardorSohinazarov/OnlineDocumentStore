using OnlineDocumentStore.Domain.Exceptions;
using System.Text.Json;

namespace OnlineDocumentStore.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        public readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate request)
            => _next = request;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
                HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(context, ex);
            }
        }

        private async void HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = "An error occurred while processing your request.",
                    details = ex.Message
                }
            }));
            return;
        }
    }
}
