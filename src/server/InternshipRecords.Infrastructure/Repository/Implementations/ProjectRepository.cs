using InternshipRecords.Domain.Entities;
using InternshipRecords.Infrastructure.Persistence;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InternshipRecords.Infrastructure.Repository.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _appDbContext;

    public ProjectRepository(AppDbContext context)
    {
        _appDbContext = context;
    }

    public async Task<Guid> CreateAsync(Project project)
    {
        _appDbContext.Projects.Add(project);
        await _appDbContext.SaveChangesAsync();
        return project.Id;
    }

    public async Task<Guid> UpdateAsync(Project project)
    {
        var existing = await _appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        if (existing == null)
            throw new KeyNotFoundException($"Проект с ID {project.Id} не найден");

        existing.Name = project.Name;
        existing.Description = project.Description;
        existing.UpdatedAt = project.UpdatedAt;

        await _appDbContext.SaveChangesAsync();

        return existing.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var existing = await _appDbContext.Projects
            .Include(p => p.Interns)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existing == null)
            throw new KeyNotFoundException($"Проект с ID {id} не найден");

        if (existing.Interns.Any())
            throw new InvalidOperationException("Невозможно удалить проект с привязанными стажерами");

        _appDbContext.Projects.Remove(existing);
        await _appDbContext.SaveChangesAsync();

        return id;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Projects
                   .Include(p => p.Interns)
                   .FirstOrDefaultAsync(p => p.Id == id)
               ?? throw new KeyNotFoundException($"Не найден проект с ID {id}");
    }

    public async Task<ICollection<Project>> GetAllAsync(params string[] queryParams)
    {
        IQueryable<Project> query = _appDbContext.Projects
            .Include(d => d.Interns);

        if (queryParams.Contains("orderByName"))
            query = query.OrderBy(d => d.Name);

        if (queryParams.Contains("orderByCount"))
            query = query.OrderByDescending(d => d.Interns.Count);

        return await query.ToListAsync();
    }

    public async Task AttachInternsAsync(Guid projectId, Guid[] internIds)
    {
        var project = await _appDbContext.Projects
            .Include(p => p.Interns)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new KeyNotFoundException($"Проект с ID {projectId} не найден");

        var interns = await _appDbContext.Interns
            .Where(i => internIds.Contains(i.Id))
            .ToListAsync();

        foreach (var intern in interns.Where(intern => !project.Interns.Contains(intern)))
        {
            project.Interns.Add(intern);
            intern.ProjectId = projectId;
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }
}