using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.GetProjects;

public record GetProjectsQuery(params string[] QueryParams) : IRequest<MbResult<ICollection<ProjectDto>>>;