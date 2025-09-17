using MediatR;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.GetIntern;

public record GetInternQuery(Guid Id) : IRequest<InternDto>;