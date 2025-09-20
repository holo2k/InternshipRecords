using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Extensions;

public static class ControllerExtensions
{
    public static IActionResult FromException(this ControllerBase controller, Exception exception)
    {
        object errorResponse;

        switch (exception)
        {
            case ValidationException ve:
                errorResponse = new
                {
                    message = "Ошибка валидации",
                    errors = ve.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                };
                return controller.UnprocessableEntity(errorResponse); // 422

            case KeyNotFoundException knf:
                errorResponse = new { message = knf.Message };
                return controller.NotFound(errorResponse); // 404

            case ArgumentNullException ane:
                errorResponse = new { message = ane.Message };
                return controller.BadRequest(errorResponse); // 400

            case ArgumentException ae:
                errorResponse = new { message = ae.Message };
                return controller.BadRequest(errorResponse); // 400

            case InvalidOperationException ioe:
                errorResponse = new { message = ioe.Message };
                return controller.Conflict(errorResponse); // 409

            default:
                errorResponse = new
                {
                    message = "Произошла ошибка на сервере",
                    detail = exception.Message,
                    type = exception.GetType().Name
                };
                return controller.StatusCode(500, errorResponse); // 500
        }
    }
}