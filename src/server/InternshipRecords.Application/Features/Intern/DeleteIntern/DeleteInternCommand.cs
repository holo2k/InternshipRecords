using MediatR;

namespace InternshipRecords.Application.Features.Intern.DeleteIntern;

public record DeleteInternCommand(Guid InternId) : IRequest<Unit>;