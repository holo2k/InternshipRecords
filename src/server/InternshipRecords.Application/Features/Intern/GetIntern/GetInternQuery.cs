using MediatR;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public record GetInternQuery(Guid Id) : IRequest<InternDto>;