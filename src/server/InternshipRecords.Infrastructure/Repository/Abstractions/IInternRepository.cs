using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Infrastructure.Repository.Abstractions;

public interface IInternRepository
{
    Task<Guid> CreateAsync(Intern intern);
    Task<Guid> UpdateAsync(Intern intern);
    Task<Guid> DeleteAsync(Guid id);
    Task<Intern?> GetByIdAsync(Guid id);
    Task<ICollection<Intern>> GetAllAsync(Guid? directionId, Guid? projectId);
}