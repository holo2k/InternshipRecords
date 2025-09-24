using AutoMapper;
using InternshipRecords.Application.Interfaces;
using InternshipRecords.Domain.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, MbResult<ProjectDto>>
{
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _uow;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork uow,
        IInternRepository internRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _uow = uow;
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var project = await _projectRepository.GetByIdAsync(request.Project.Id);

            project!.Name = request.Project.Name;
            project.Description = request.Project.Description;
            project.UpdatedAt = DateTime.UtcNow;
            await _projectRepository.UpdateAsync(project);

            var internsToAssign = await _internRepository.GetManyAsync(request.Project.InternIds!);
            foreach (var intern in internsToAssign) intern.ProjectId = request.Project.Id;

            var previously = await _internRepository.GetByProjectIdAsync(request.Project.Id);
            var toRemove = previously.Where(i => !request.Project.InternIds!.Contains(i.Id)).ToList();
            foreach (var intern in toRemove) intern.ProjectId = null;

            await _uow.SaveChangesAsync(cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            return MbResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync(cancellationToken);

            return ex switch
            {
                KeyNotFoundException => MbResult<ProjectDto>.Fail(new MbError("NotFound", ex.Message)),
                _ => MbResult<ProjectDto>.Fail(new MbError("Неизвестное исключение", ex.Message))
            };
        }
    }
}