using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public record UpdateInternCommand(UpdateInternRequest Intern) : IRequest<MbResult<Guid>>;