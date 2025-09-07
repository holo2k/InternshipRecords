using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Guid> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Project.Id);
        if (project is null)
            throw new KeyNotFoundException($"Project with id {request.Project.Id} not found");

        project.Name = request.Project.Name;
        project.Description = request.Project.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.SaveAsync();

        return project.Id;
    }
}