using MediatR;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public record UpdateProjectCommand(UpdateProjectRequest Project) : IRequest<Guid>;