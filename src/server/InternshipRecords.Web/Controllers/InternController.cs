using InternshipRecords.Application.Features.Intern.AddIntern;
using InternshipRecords.Application.Features.Intern.DeleteIntern;
using InternshipRecords.Application.Features.Intern.GetIntern;
using InternshipRecords.Application.Features.Intern.GetInterns;
using InternshipRecords.Application.Features.Intern.UpdateIntern;
using InternshipRecords.Web.Extensions;
using InternshipRecords.Web.Hub;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared.Models.Intern;

namespace InternshipRecords.Web.Controllers;

/// <summary>
///     API для управления стажёрами.
///     Отправляет уведомления в SignalR-хаб при создании, обновлении и удалении стажёров.
/// </summary>
[ApiController]
[Route("api/intern")]
public class InternController : ControllerBase
{
    private readonly IHubContext<InternsHub> _hub;
    private readonly IMediator _mediator;

    public InternController(IMediator mediator, IHubContext<InternsHub> hub)
    {
        _mediator = mediator;
        _hub = hub;
    }

    /// <summary>
    ///     Получить стажёра по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор стажёра.</param>
    /// <returns>DTO стажёра.</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var query = new GetInternQuery(id);
            var intern = await _mediator.Send(query);
            return Ok(intern);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Получить список стажёров.
    ///     Поддерживаются фильтры по directionId и projectId.
    /// </summary>
    /// <param name="directionId">Фильтр по направлению (опционально).</param>
    /// <param name="projectId">Фильтр по проекту (опционально).</param>
    /// <returns>Список стажёров.</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<InternDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? directionId, [FromQuery] Guid? projectId)
    {
        try
        {
            var query = new GetInternsQuery(directionId, projectId);
            var list = await _mediator.Send(query);
            return Ok(list);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Создать нового стажёра.
    ///     Отправляет событие InternCreated в SignalR-хаб с созданным DTO.
    /// </summary>
    /// <param name="request">Данные для создания стажёра.</param>
    /// <returns>Созданный DTO стажёра.</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] AddInternRequest request)
    {
        try
        {
            var id = await _mediator.Send(new AddInternCommand(request));
            var created = await _mediator.Send(new GetInternQuery(id));
            await _hub.Clients.All.SendAsync("InternCreated", created);
            return Ok(created);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Обновить существующего стажёра.
    ///     Отправляет событие InternUpdated в SignalR-хаб с обновлённым DTO.
    /// </summary>
    /// <param name="request">Данные для обновления (включая Id).</param>
    /// <returns>Обновлённый DTO стажёра.</returns>
    [HttpPatch]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InternDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateInternRequest request)
    {
        try
        {
            var id = await _mediator.Send(new UpdateInternCommand(request));
            var updated = await _mediator.Send(new GetInternQuery(id));
            await _hub.Clients.All.SendAsync("InternUpdated", updated);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }

    /// <summary>
    ///     Удалить стажёра по идентификатору.
    ///     Отправляет событие InternDeleted в SignalR-хаб с Id удалённого.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого стажёра.</param>
    /// <returns>200 OK при успешном удалении.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteInternCommand(id));
            await _hub.Clients.All.SendAsync("InternDeleted", id);
            return Ok();
        }
        catch (Exception ex)
        {
            return this.FromException(ex);
        }
    }
}