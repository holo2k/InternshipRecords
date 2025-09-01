using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Infrastructure.Repository.Abstractions;

public interface IDirectionRepository
{
    Task<Guid> CreateAsync(Direction direction);
    Task<Guid> UpdateAsync(Direction direction);
    Task<Guid> DeleteAsync(Guid id);
    Task<Direction?> GetByIdAsync(Guid id);
    Task<ICollection<Direction>> GetAll(params string[] queryParams);
    Task AttachInterns(Guid directionId, Guid[] internIds);
}