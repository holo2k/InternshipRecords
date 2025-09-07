using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Infrastructure.Repository.Abstractions;

public interface IProjectRepository
{
    Task<Guid> CreateAsync(Project project);
    Task<Guid> UpdateAsync(Project project);
    Task<Guid> DeleteAsync(Guid id);
    Task<Project?> GetByIdAsync(Guid id);
    Task<ICollection<Project>> GetAllAsync(params string[] queryParams);
    Task AttachInternsAsync(Guid projectId, Guid[] internIds);
    Task SaveAsync();
}