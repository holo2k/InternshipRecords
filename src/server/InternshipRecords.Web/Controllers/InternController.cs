using InternshipRecords.Application.Features.Intern.AddIntern;
using InternshipRecords.Application.Features.Intern.DeleteIntern;
using InternshipRecords.Application.Features.Intern.GetIntern;
using InternshipRecords.Application.Features.Intern.GetInterns;
using InternshipRecords.Application.Features.Intern.UpdateIntern;
using InternshipRecords.Web.Hub;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared.Models.Intern;

namespace InternshipRecords.Web.Controllers;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAll([FromQuery] Guid id)
    {
        var query = new GetInternQuery(id);
        var intern = await _mediator.Send(query);
        return Ok(intern);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? directionId, [FromQuery] Guid? projectId)
    {
        var query = new GetInternsQuery(directionId, projectId);
        var list = await _mediator.Send(query);
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddInternRequest request)
    {
        var id = await _mediator.Send(new AddInternCommand(request));
        var created = await _mediator.Send(new GetInternQuery(id));
        await _hub.Clients.All.SendAsync("InternCreated", created);
        return Ok(created);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateInternRequest request)
    {
        var id = await _mediator.Send(new UpdateInternCommand(request));
        var updated = await _mediator.Send(new GetInternQuery(id));
        await _hub.Clients.All.SendAsync("InternUpdated", updated);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteInternCommand(id));
        await _hub.Clients.All.SendAsync("InternDeleted", id);
        return Ok();
    }
}