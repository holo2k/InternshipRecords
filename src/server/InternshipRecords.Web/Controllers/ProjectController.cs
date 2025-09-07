using InternshipRecords.Application.Features.Project;
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
    public async Task<IEnumerable<ProjectDto>> GetAll(GetProjectsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpPost]
    public async Task<Guid> Create([FromBody] AddProjectCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPatch]
    public async Task<Guid> Update([FromBody] UpdateProjectCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<Unit> Delete(Guid id)
    {
        return await _mediator.Send(new DeleteProjectCommand(id));
    }

    [HttpPost("attach-interns")]
    public async Task<Unit> AttachInterns([FromBody] AttachInternsToProjectCommand command)
    {
        return await _mediator.Send(command);
    }
}