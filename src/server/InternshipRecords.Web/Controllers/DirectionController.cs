using InternshipRecords.Application.Features.Direction.AddDirection;
using InternshipRecords.Application.Features.Direction.AttachInternsToDirection;
using InternshipRecords.Application.Features.Direction.DeleteDirection;
using InternshipRecords.Application.Features.Direction.GetDirections;
using InternshipRecords.Application.Features.Direction.UpdateDirection;
using InternshipRecords.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Web.Controllers;

/// <summary>
///     API для управления направлениями.
/// </summary>
[Route("api/direction")]
[ApiController]
public class DirectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public DirectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Получить список направлений.
    ///     Поддерживает дополнительные query-параметры (например "orderByName", "orderByCount").
    /// </summary>
    /// <param name="queryParams">Массив query-параметров.</param>
    /// <returns>Список направлений (в теле ответа возвращается результат, формируемый обработчиком).</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] string[] queryParams)
    {
        try
        {
            var query = new GetDirectionsQuery
            {
                QueryParams = queryParams
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Создать новое направление.
    /// </summary>
    /// <param name="request">Данные нового направления.</param>
    /// <returns>Результат создания (обычно возвращается созданный объект или идентификатор).</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] AddDirectionRequest request)
    {
        try
        {
            var command = new AddDirectionCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Обновить существующее направление.
    /// </summary>
    /// <param name="request">Модель обновления направления, включая Id.</param>
    /// <returns>Результат обновления.</returns>
    [HttpPatch]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateDirectionRequest request)
    {
        try
        {
            var command = new UpdateDirectionCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Удалить направление по идентификатору.
    ///     Если направление не может быть удалено (например есть связанные стажёры),
    ///     обработчик может вернуть специальный результат (например Guid.Empty).
    /// </summary>
    /// <param name="id">Id удаляемого направления.</param>
    /// <returns>Идентификатор удалённого направления или индикатор ошибки.</returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteDirectionCommand(id));
        return Ok(result);
    }

    /// <summary>
    ///     Прикрепить список стажёров к направлению.
    ///     Ожидает команду AttachInternsToDirectionCommand в теле запроса.
    /// </summary>
    /// <param name="command">Команда с Id направления и списком Id стажёров.</param>
    /// <returns>Результат операции прикрепления.</returns>
    [HttpPost("attach-interns")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AttachInterns([FromBody] AttachInternsToDirectionCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }
}