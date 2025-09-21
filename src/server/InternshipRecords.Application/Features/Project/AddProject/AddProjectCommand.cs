using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.AddProject;

public record AddProjectCommand(AddProjectRequest Project) : IRequest<MbResult<ProjectDto>>;