using MediatR;
using Shared.Models.Intern;

namespace InternshipRecords.Application.Features.Intern.AddIntern;

public record AddInternCommand(AddInternRequest Intern) : IRequest<Guid>;