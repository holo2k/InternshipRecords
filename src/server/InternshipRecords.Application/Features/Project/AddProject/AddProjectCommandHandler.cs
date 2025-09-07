using AutoMapper;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Project.AddProject;

public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;

    public AddProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Project>(request.Project);

        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.CreateAsync(entity);

        return entity.Id;
    }
}