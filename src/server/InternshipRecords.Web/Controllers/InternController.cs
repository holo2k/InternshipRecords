using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Controllers;

[Route("api/intern")]
[ApiController]
public class InternController : ControllerBase
{
    private readonly IMediator _mediator;

    public InternController(IMediator mediator)
    {
        _mediator = mediator;
    }
}