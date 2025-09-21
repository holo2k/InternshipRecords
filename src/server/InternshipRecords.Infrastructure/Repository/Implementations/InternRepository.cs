using InternshipRecords.Domain.Entities;
using InternshipRecords.Infrastructure.Persistence;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InternshipRecords.Infrastructure.Repository.Implementations;

public class InternRepository : IInternRepository
{
    private readonly AppDbContext _appDbContext;

    public InternRepository(AppDbContext context)
    {
        _appDbContext = context;
    }

    public async Task<Guid> CreateAsync(Intern intern)
    {
        if (_appDbContext.Interns.Any(i => i.Email == intern.Email || i.Phone == intern.Phone))
            throw new ArgumentException("Стажёр с такой почтой или телефоном уже существует");
        _appDbContext.Interns.Add(intern);
        await _appDbContext.SaveChangesAsync();
        return intern.Id;
    }

    public async Task<Guid> UpdateAsync(Intern intern)
    {
        var existing = await _appDbContext.Interns
            .FirstOrDefaultAsync(i => i.Id == intern.Id);

        if (existing == null)
            throw new KeyNotFoundException($"Стажёр с ID {intern.Id} не найден");

        if (intern.DirectionId.HasValue &&
            await _appDbContext.Directions.FindAsync(intern.DirectionId) == null)
            throw new KeyNotFoundException($"Направление с ID {intern.DirectionId} не найдено");

        if (intern.ProjectId.HasValue &&
            await _appDbContext.Projects.FindAsync(intern.ProjectId) == null)
            throw new KeyNotFoundException($"Проект с ID {intern.ProjectId} не найден");

        _appDbContext.Interns.Update(intern);
        await _appDbContext.SaveChangesAsync();

        return existing.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var existing = await _appDbContext.Interns
            .FirstOrDefaultAsync(i => i.Id == id);

        if (existing == null)
            throw new KeyNotFoundException($"Стажёр с ID {id} не найден");

        _appDbContext.Interns.Remove(existing);
        await _appDbContext.SaveChangesAsync();

        return id;
    }

    public async Task<Intern> GetByIdAsync(Guid id)
    {
        var intern = await _appDbContext.Interns
                         .Include(i => i.Direction)
                         .Include(i => i.Project)
                         .FirstOrDefaultAsync(i => i.Id == id)
                     ?? throw new KeyNotFoundException($"Стажёр с ID {id} не найден");
        return intern;
    }

    public async Task<ICollection<Intern>> GetAllAsync(Guid? directionId, Guid? projectId)
    {
        IQueryable<Intern> query = _appDbContext.Interns
            .Include(i => i.Direction)
            .Include(i => i.Project);

        if (directionId.HasValue)
            query = query.Where(i => i.DirectionId == directionId.Value);

        if (projectId.HasValue)
            query = query.Where(i => i.ProjectId == projectId.Value);

        return await query.ToListAsync();
    }

    public async Task<ICollection<Intern>> GetManyAsync(List<Guid> internIds)
    {
        var query = _appDbContext.Interns.Where(i => internIds.Contains(i.Id));
        return await query.ToListAsync();
    }

    public async Task<ICollection<Intern>> GetByDirectionIdAsync(Guid directionId)
    {
        var query = _appDbContext.Interns.Where(i => i.DirectionId == directionId);
        return await query.ToListAsync();
    }

    public async Task<ICollection<Intern>> GetByProjectIdAsync(Guid projectId)
    {
        var query = _appDbContext.Interns.Where(i => i.ProjectId == projectId);
        return await query.ToListAsync();
    }
}