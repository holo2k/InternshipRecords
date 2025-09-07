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
        _appDbContext.Interns.Add(intern);
        await _appDbContext.SaveChangesAsync();
        return intern.Id;
    }

    public async Task<Guid> UpdateAsync(Intern intern)
    {
        var existing = await _appDbContext.Interns
            .FirstOrDefaultAsync(i => i.Id == intern.Id);

        if (existing == null)
            throw new KeyNotFoundException($"Intern with id {intern.Id} not found");

        if (intern.DirectionId.HasValue &&
            await _appDbContext.Directions.FindAsync(intern.DirectionId) == null)
            throw new KeyNotFoundException($"Direction with id {intern.DirectionId} not found");

        if (intern.ProjectId.HasValue &&
            await _appDbContext.Directions.FindAsync(intern.ProjectId) == null)
            throw new KeyNotFoundException($"Project with id {intern.ProjectId} not found");

        _appDbContext.Interns.Update(intern);
        await _appDbContext.SaveChangesAsync();

        return existing.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var existing = await _appDbContext.Interns
            .FirstOrDefaultAsync(i => i.Id == id);

        if (existing == null)
            throw new KeyNotFoundException($"Intern with id {id} not found");

        _appDbContext.Interns.Remove(existing);
        await _appDbContext.SaveChangesAsync();

        return id;
    }

    public async Task<Intern?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Interns
            .Include(i => i.Direction)
            .Include(i => i.Project)
            .FirstOrDefaultAsync(i => i.Id == id);
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
}