using MediatR;
using Shared.Models;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public record GetInternQuery(Guid Id) : IRequest<MbResult<InternDto>>;