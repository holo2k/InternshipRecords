using MediatR;

namespace InternshipRecords.Application.Features.Intern.UpdateIntern;

public record UpdateInternCommand(UpdateInternRequest Intern) : IRequest<Guid>;