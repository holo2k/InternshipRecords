using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Domain.Repository.Abstractions;

public interface IProjectRepository
{
    Task<Guid> CreateAsync(Project project);
    Task<Guid> UpdateAsync(Project project);
    Task<Guid> DeleteAsync(Guid id);
    Task<Project?> GetByIdAsync(Guid id);
    Task<ICollection<Project>> GetAllAsync(params string[] queryParams);
}