using ConsignmentSystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ConsignmentSystem.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Title = "Validation Error";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = "One or more validation errors occurred. Please check the 'errors' property for details.";

                problemDetails.Extensions.Add("errors", validationException.Errors);
            }
            else if (exception is InvalidOperationException invalidOpException)
            {
                problemDetails.Title = "Business Rule Violation";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = invalidOpException.Message;
            }
            else if (exception is NotFoundException notFoundException)
            {
                problemDetails.Title = "Resource Not Found";
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = notFoundException.Message;
            }
            else
            {
                problemDetails.Title = "An unexpected error occurred";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "Please contact the system administrator.";
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
