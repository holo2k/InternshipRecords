using InternshipRecords.Domain.Entities;

namespace InternshipRecords.Infrastructure.Repository.Abstractions;

public interface IProjectRepository
{
    Task<Guid> CreateAsync(Project project);
    Task<Guid> UpdateAsync(Project project);
    Task<Guid> DeleteAsync(Guid id);
    Task<Project?> GetByIdAsync(Guid id);
    Task<ICollection<Project>> GetAll(params string[] queryParams);
    Task AttachInterns(Guid projectId, Guid[] internIds);
}