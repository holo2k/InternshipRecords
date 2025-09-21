using AutoMapper;
using InternshipRecords.Infrastructure.Persistence;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using MediatR;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Application.Features.Project.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, MbResult<ProjectDto>>
{
    private readonly AppDbContext _db;
    private readonly IInternRepository _internRepository;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, AppDbContext db,
        IInternRepository internRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _db = db;
        _internRepository = internRepository;
        _mapper = mapper;
    }

    public async Task<MbResult<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
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

            await _db.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return MbResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(cancellationToken);

            return ex switch
            {
                KeyNotFoundException => MbResult<ProjectDto>.Fail(new MbError("NotFound", ex.Message)),
                _ => MbResult<ProjectDto>.Fail(new MbError("Неизвестное исключение", ex.Message))
            };
        }
    }
}