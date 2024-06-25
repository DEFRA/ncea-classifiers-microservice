using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ncea.Classifier.Microservice.ExceptionHandlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
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
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails();
        switch (exception)
        {
            case ArgumentNullException argumentNullException:
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = argumentNullException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = argumentNullException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                _logger.LogError(argumentNullException, $"Exception occured : {argumentNullException.Message}");
                break;

            case ValidationException validationException:
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = validationException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = validationException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                _logger.LogError(validationException, $"Exception occured : {validationException.Message}");
                break;

            case UnauthorizedAccessException authorizedException:
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = authorizedException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = authorizedException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                _logger.LogError(authorizedException, $"Exception occured : {authorizedException.Message}");
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };
                _logger.LogError(exception, $"Exception occured : {exception.Message}");
                break;
        }

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
