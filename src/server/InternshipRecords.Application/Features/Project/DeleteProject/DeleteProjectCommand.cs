using MediatR;

namespace InternshipRecords.Application.Features.Project.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : IRequest<Guid>;