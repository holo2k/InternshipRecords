using MediatR;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.AddProject;

public record AddProjectCommand(AddProjectRequest Project) : IRequest<ProjectDto>;