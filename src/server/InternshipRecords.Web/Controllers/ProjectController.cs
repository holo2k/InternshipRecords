using InternshipRecords.Application.Features.Project.AddProject;
using InternshipRecords.Application.Features.Project.DeleteProject;
using InternshipRecords.Application.Features.Project.GetProjects;
using InternshipRecords.Application.Features.Project.UpdateProject;
using InternshipRecords.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Web.Controllers;

/// <summary>
///     API для управления проектами.
/// </summary>
[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Получить список проектов.
    ///     Поддерживает дополнительные query-параметры (например "orderByName", "orderByCount").
    /// </summary>
    /// <param name="queryParams">Массив query-параметров.</param>
    /// <returns>Список проектов или результат, формируемый обработчиком.</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] string[] queryParams)
    {
        var query = new GetProjectsQuery
        {
            QueryParams = queryParams
        };
        var result = await _mediator.Send(query);
        return this.FromResult(result);
    }

    /// <summary>
    ///     Создать новый проект.
    /// </summary>
    /// <param name="request">Данные для создания проекта.</param>
    /// <returns>Результат создания (обычно созданный объект или идентификатор).</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] AddProjectRequest request)
    {
        var command = new AddProjectCommand(request);
        var result = await _mediator.Send(command);
        return this.FromResult(result);
    }

    /// <summary>
    ///     Обновить существующий проект.
    /// </summary>
    /// <param name="request">Модель обновления проекта, включая Id.</param>
    /// <returns>Результат обновления.</returns>
    [HttpPatch]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request)
    {
        var command = new UpdateProjectCommand(request);
        var result = await _mediator.Send(command);
        return this.FromResult(result);
    }

    /// <summary>
    ///     Удалить проект по идентификатору.
    ///     Если проект не может быть удалён (например есть связанные стажёры),
    ///     обработчик может вернуть специальный результат (например Guid.Empty).
    /// </summary>
    /// <param name="id">Id удаляемого проекта.</param>
    /// <returns>Идентификатор удалённого проекта или индикатор ошибки.</returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id));
        return this.FromResult(result);
    }
}