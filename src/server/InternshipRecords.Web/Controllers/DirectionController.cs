using InternshipRecords.Application.Features.Direction.AddDirection;
using InternshipRecords.Application.Features.Direction.AttachInternsToDirection;
using InternshipRecords.Application.Features.Direction.DeleteDirection;
using InternshipRecords.Application.Features.Direction.GetDirections;
using InternshipRecords.Application.Features.Direction.UpdateDirection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Controllers;

[Route("api/direction")]
[ApiController]
public class DirectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public DirectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string[] queryParams)
    {
        var query = new GetDirectionsQuery
        {
            QueryParams = queryParams
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddDirectionCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateDirectionCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteDirectionCommand(id)));
    }

    [HttpPost("attach-interns")]
    public async Task<IActionResult> AttachInterns([FromBody] AttachInternsToDirectionCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}