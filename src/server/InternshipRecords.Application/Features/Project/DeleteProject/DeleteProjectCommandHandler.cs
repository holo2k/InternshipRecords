using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;

namespace InternshipRecords.Application.Features.Project.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, MbResult<Guid>>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<MbResult<Guid>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = MbResult<Guid>.Success(await _projectRepository.DeleteAsync(request.ProjectId));
            return result;
        }
        catch (Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => MbResult<Guid>.Fail(new MbError("NotFound", ex.Message)),
                InvalidOperationException => MbResult<Guid>.Fail(new MbError("ObjectHasLinkedEntities", ex.Message)),
                _ => MbResult<Guid>.Fail(new MbError("Неизвестное исключение", ex.Message))
            };
        }
    }
}