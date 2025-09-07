using MediatR;

namespace InternshipRecords.Application.Features.Intern.GetInterns;

public record GetInternsQuery(Guid? ProjectId = null, Guid? DirectionId = null) : IRequest<ICollection<InternDto>>;