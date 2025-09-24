using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Domain.Repository.Abstractions;

public interface IDirectionRepository
{
    Task<Guid> CreateAsync(Direction direction);
    Task<Guid> UpdateAsync(Direction direction);
    Task<Guid> DeleteAsync(Guid id);
    Task<Direction?> GetByIdAsync(Guid id);
    Task<ICollection<Direction>> GetAllAsync(params string[] queryParams);
    Task SaveAsync();
}