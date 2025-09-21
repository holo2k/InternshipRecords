using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.GetInterns;

public record GetInternsQuery
    (Guid? ProjectId = null, Guid? DirectionId = null) : IRequest<MbResult<ICollection<InternDto>>>;