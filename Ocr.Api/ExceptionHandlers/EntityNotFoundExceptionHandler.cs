﻿using Microsoft.AspNetCore.Mvc;
using Ocr.Api.ExceptionHandlers.Abstract;
using Ocr.Services.Exceptions.Abstract;

namespace Ocr.Api.ExceptionHandlers;

public class EntityNotFoundExceptionHandler : BaseExceptionHandler<EntityNotFoundException>
{
    public EntityNotFoundExceptionHandler(ILogger<EntityNotFoundException> logger) : base(logger)
    {
    }

    public override async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EntityNotFoundException entityNotFoundException)
            return false;

        Logger.LogError(entityNotFoundException, entityNotFoundException.Message, exception.Message,
            exception.InnerException?.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Entity not found",
            Detail = entityNotFoundException.Message,
            Status = 404
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}