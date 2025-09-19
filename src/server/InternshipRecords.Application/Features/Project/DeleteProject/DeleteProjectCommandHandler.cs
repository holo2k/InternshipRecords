using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Project.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Guid> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        return await _projectRepository.DeleteAsync(request.ProjectId);
    }
}