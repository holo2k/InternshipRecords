using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Extensions;

public static class ControllerExtensions
{
    public static IActionResult FromException(this ControllerBase controller, Exception exception)
    {
        var error = new
        {
            message = exception.Message,
            type = exception.GetType().Name
        };

        return exception switch
        {
            KeyNotFoundException => controller.NotFound(error), // 404
            ValidationException => controller.UnprocessableEntity(error), // 422
            ArgumentNullException => controller.BadRequest(error), // 400
            ArgumentException => controller.BadRequest(error), // 400
            InvalidOperationException => controller.Conflict(error), //409
            _ => controller.StatusCode(500, error) // 500
        };
    }
}