using InternshipRecords.Application.Features.Direction;
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
    public async Task<IEnumerable<DirectionDto>> GetAll(GetDirectionsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpPost]
    public async Task<Guid> Create([FromBody] AddDirectionCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPatch]
    public async Task<Guid> Update([FromBody] UpdateDirectionCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<Unit> Delete(Guid id)
    {
        return await _mediator.Send(new DeleteDirectionCommand(id));
    }

    [HttpPost("attach-interns")]
    public async Task<Unit> AttachInterns([FromBody] AttachInternsToDirectionCommand command)
    {
        return await _mediator.Send(command);
    }
}