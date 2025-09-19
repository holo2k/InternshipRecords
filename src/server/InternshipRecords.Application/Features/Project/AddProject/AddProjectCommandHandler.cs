using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.AddProject;

public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, ProjectDto>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;

    public AddProjectCommandHandler(
        IProjectRepository projectRepository,
        IInternRepository internRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Project>(request.Project);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        if (request.Project.InternIds.Any())
        {
            var interns = await _internRepository.GetManyAsync(request.Project.InternIds);
            entity.Interns = interns.ToList();
        }

        await _projectRepository.CreateAsync(entity);

        return _mapper.Map<ProjectDto>(entity);
    }
}