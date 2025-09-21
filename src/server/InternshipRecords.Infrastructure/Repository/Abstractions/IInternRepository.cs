using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Infrastructure.Repository.Abstractions;

public interface IInternRepository
{
    Task<Guid> CreateAsync(Intern intern);
    Task<Guid> UpdateAsync(Intern intern);
    Task<Guid> DeleteAsync(Guid id);
    Task<Intern> GetByIdAsync(Guid id);
    Task<ICollection<Intern>> GetAllAsync(Guid? directionId, Guid? projectId);
    Task<ICollection<Intern>> GetManyAsync(List<Guid> internIds);
    Task<ICollection<Intern>> GetByDirectionIdAsync(Guid directionId);
    Task<ICollection<Intern>> GetByProjectIdAsync(Guid projectId);
}