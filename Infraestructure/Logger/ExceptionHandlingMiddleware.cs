using BootcampCLT.Api.Response;
using BootcampCLT.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ValidationException = BootcampCLT.Application.Exceptions.ValidationException;

namespace BootcampCLT.Infraestructure.Logger
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ApiBadRequestException e)
            {
                _logger.LogWarning("{Codigo}, {Error}", e.ErrorCode, e.Message);

                await HandleExceptionAsync(context, e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var errors = GetErrors(exception);
            KeyValuePair<string, string[]> errorData;
            ErrorResponse response = new();

            if (errors != null)
            {
                errorData = errors.AsQueryable().First();
                response = new ErrorResponse
                {
                    Codigo = errorData.Key,
                    Mensaje = string.Join("|", errorData.Value)
                };
            }

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            static IReadOnlyDictionary<string, string[]> defError(string code, string des)
                => new Dictionary<string, string[]>()
                {
                { code,new string[] { des } }
                };

            var innerException = exception.InnerException?.Message;

            if (innerException != null)
            {
                var index = innerException.IndexOf('|');

                if (index != -1)
                {
                    innerException = innerException[..index];
                }

                var innerExceptionMessage = exception.InnerException?.InnerException?.Message;

                innerException = exception.InnerException?.InnerException != null
                    ? innerException + "| " + innerExceptionMessage
                    : innerException.TrimEnd() + ".";
            }

            return exception switch
            {
                ValidationException validException => validException.ErrorsDictionary,
                ApiBadRequestException BadReqException => BadReqException.ErrorsDictionary,
                ApiException apiEx => apiEx.ErrorsDictionary,
                _ => exception.InnerException?.Source == "System.Text.Json"
                    ? defError("500", innerException ?? "Internal error")
                    : defError("500", exception.Message)
            };
        }
    }
}
