using EY_Test.Models;
using Serilog;
namespace EY_Test.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            DateTime startDateTime = DateTime.Now;
            Log.Information($"[Start] Path: ${context.Request.Path} \n TraceId:\n ${context.TraceIdentifier}");
            try
            {
                await _next(context);
            }
            catch (MyException ex)
            {
                Log.Error(ex.MSG, "An unexpected error occurred.");
                await HandleMyExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected error occurred.");
                Log.Error(ex, "An unexpected error occurred.");
                
            }
            DateTime endDateTime = DateTime.Now;
            Log.Information($"[End] Path: ${context.Request.Path} \n TraceId:\n ${context.TraceIdentifier}\n Duration:\n${endDateTime-startDateTime}");
        }

        private Task HandleMyExceptionAsync(HttpContext context, MyException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new { Message = exception.MSG, StatusCode = context.Response.StatusCode };
            
            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
