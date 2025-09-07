using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;

namespace InternshipRecords.Application.Features.Project.AttachInternsToProject;

public class AttachInternsToProjectCommandHandler : IRequestHandler<AttachInternsToProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;

    public AttachInternsToProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(AttachInternsToProjectCommand request, CancellationToken cancellationToken)
    {
        await _projectRepository.AttachInternsAsync(request.ProjectId, request.InternIds);
        return Unit.Value;
    }
}