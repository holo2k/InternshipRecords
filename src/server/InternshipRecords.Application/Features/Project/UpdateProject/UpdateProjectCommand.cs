using MediatR;
using Shared.Models;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public record UpdateProjectCommand(UpdateProjectRequest Project) : IRequest<MbResult<ProjectDto>>;