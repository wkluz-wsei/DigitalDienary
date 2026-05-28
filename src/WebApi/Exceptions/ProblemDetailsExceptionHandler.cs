using CoreApp.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Exceptions;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory,
    ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is InvalidOperationException)
        {
            logger.LogInformation("Exception {Message} handled!", exception.Message);
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status401Unauthorized,
                "Authentication error!",
                "Unauthorized",
                detail: exception.Message);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        if (exception is StudentNotFoundException
            or CourseNotFoundException
            or LecturerNotFoundException
            or AcademicYearNotFoundException
            or GradeNotFoundException)
        {
            logger.LogInformation("Exception {Message} handled!", exception.Message);
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                "Student service error!",
                "Resource not found",
                detail: exception.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        return false;
    }
}
