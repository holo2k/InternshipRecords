using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, ICollection<ProjectDto>>
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(request.QueryParams);
        return _mapper.Map<ICollection<ProjectDto>>(projects);
    }
}