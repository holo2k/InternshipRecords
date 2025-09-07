using MediatR;

namespace InternshipRecords.Application.Features.Project.GetProjects;

public record GetProjectsQuery(params string[] QueryParams) : IRequest<ICollection<ProjectDto>>;