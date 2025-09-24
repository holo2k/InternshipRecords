using InternshipRecords.Domain.Entities;
using InternshipRecords.Domain.Repository.Abstractions;
using InternshipRecords.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InternshipRecords.Infrastructure.Repository.Implementations;

public class DirectionRepository : IDirectionRepository
{
    private readonly AppDbContext _appDbContext;

    public DirectionRepository(AppDbContext context)
    {
        _appDbContext = context;
    }

    public async Task<Guid> CreateAsync(Direction direction)
    {
        _appDbContext.Directions.Add(direction);
        await _appDbContext.SaveChangesAsync();
        return direction.Id;
    }

    public async Task<Guid> UpdateAsync(Direction direction)
    {
        var existing = await _appDbContext.Directions
            .FirstOrDefaultAsync(d => d.Id == direction.Id);

        if (existing == null)
            throw new KeyNotFoundException($"Направление с ID {direction.Id} не найдено");

        existing.Name = direction.Name;
        existing.Description = direction.Description;
        existing.UpdatedAt = direction.UpdatedAt;

        await _appDbContext.SaveChangesAsync();

        return existing.Id;
    }


    public async Task<Guid> DeleteAsync(Guid id)
    {
        var existing = await _appDbContext.Directions
            .Include(d => d.Interns)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (existing == null)
            throw new KeyNotFoundException($"Направление с ID {id} не найдено");

        if (existing.Interns.Any())
            throw new InvalidOperationException("Невозможно удалить проект с привязанными стажерами");

        _appDbContext.Directions.Remove(existing);
        await _appDbContext.SaveChangesAsync();

        return id;
    }

    public async Task<Direction?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Directions
            .Include(d => d.Interns)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<ICollection<Direction>> GetAllAsync(params string[] queryParams)
    {
        IQueryable<Direction> query = _appDbContext.Directions
            .Include(d => d.Interns);

        if (queryParams.Contains("orderByName"))
            query = query.OrderBy(d => d.Name);

        if (queryParams.Contains("orderByCount"))
            query = query.OrderByDescending(d => d.Interns.Count);

        return await query.ToListAsync();
    }

    public Task SaveAsync()
    {
        return _appDbContext.SaveChangesAsync();
    }
}