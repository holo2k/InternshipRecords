using InternshipRecords.Application.Features.Project.AddProject;
using InternshipRecords.Application.Features.Project.AttachInternsToProject;
using InternshipRecords.Application.Features.Project.DeleteProject;
using InternshipRecords.Application.Features.Project.GetProjects;
using InternshipRecords.Application.Features.Project.UpdateProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Controllers;

[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string[] queryParams)
    {
        var query = new GetProjectsQuery
        {
            QueryParams = queryParams
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddProjectCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateProjectCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteProjectCommand(id)));
    }

    [HttpPost("attach-interns")]
    public async Task<IActionResult> AttachInterns([FromBody] AttachInternsToProjectCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}