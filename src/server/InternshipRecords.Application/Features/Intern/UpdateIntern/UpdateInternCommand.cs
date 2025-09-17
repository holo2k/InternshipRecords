using MediatR;
using Shared.Models;

rn;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public record UpdateInternCommand(UpdateInternRequest Intern) : IRequest<Guid>;