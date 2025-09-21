using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, MbResult<ICollection<ProjectDto>>>
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<ICollection<ProjectDto>>> Handle(GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var projects = await _projectRepository.GetAllAsync(request.QueryParams);
            return MbResult<ICollection<ProjectDto>>.Success(_mapper.Map<ICollection<ProjectDto>>(projects));
        }
        catch (Exception ex)
        {
            return MbResult<ICollection<ProjectDto>>.Fail(new MbError("Неизвестное исключение", ex.Message));
        }
    }
}