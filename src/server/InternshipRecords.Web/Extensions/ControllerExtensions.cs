using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace InternshipRecords.Web.Extensions;

public static class ControllerExtensions
{
    public static IActionResult FromResult<T>(this ControllerBase controller, MbResult<T> result)
    {
        if (result.IsSuccess) return controller.Ok(result);

        var logger = controller.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerBase>>();
        logger.LogError("Error: {ErrorCode}, Message: {ErrorMessage}, Data: {@Result}",
            result.Error?.Code,
            result.Error?.Message,
            result);

        return result.Error?.Code switch
        {
            "NotFound" => controller.NotFound(result), //404
            "ValidationFailure" => controller.UnprocessableEntity(result), //422
            "ObjectHasLinkedEntities" => controller.Conflict(result), //409
            "NotReady" => controller.StatusCode(503, result), //503
            _ => controller.StatusCode(500, result) //500
        };
    }
}