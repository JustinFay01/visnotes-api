using Microsoft.AspNetCore.Mvc;
using Ocr.Api.ExceptionHandlers.Abstract;
using Ocr.Services.SystemStorage.Exceptions;

namespace Ocr.Api.ExceptionHandlers;

public class FileConflictExceptionHandler(ILogger<FileConflictExceptionHandler> logger)
    : BaseExceptionHandler<FileConflictExceptionHandler>(logger)
{
    public override async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not FileExistsException fileExistsException)
            return false;

        Logger.LogError(fileExistsException, fileExistsException.Message, exception.Message,
            exception.InnerException?.Message);

        var problemDetails = new ProblemDetails
        {
            Title =  "File conflict",
            Detail = "File already exists.",
            Status = 409
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}