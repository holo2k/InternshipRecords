using AutoMapper;
using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.AddProject;

public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, MbResult<ProjectDto>>
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

    public async Task<MbResult<ProjectDto>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        try
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

            return MbResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(entity));
        }
        catch (Exception ex)
        {
            return MbResult<ProjectDto>.Fail(new MbError("Неизвестное исключение", ex.Message));
        }
    }
}