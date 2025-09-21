using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Project.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : IRequest<MbResult<Guid>>;