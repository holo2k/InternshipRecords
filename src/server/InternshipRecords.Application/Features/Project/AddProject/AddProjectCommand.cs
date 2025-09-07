using MediatR;

namespace InternshipRecords.Application.Features.Project.AddProject;

public record AddProjectCommand(AddProjectRequest Project) : IRequest<Guid>;