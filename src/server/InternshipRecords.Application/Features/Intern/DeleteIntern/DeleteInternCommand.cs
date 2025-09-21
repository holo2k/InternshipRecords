using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Intern.DeleteIntern;

public record DeleteInternCommand(Guid InternId) : IRequest<MbResult<Guid>>;