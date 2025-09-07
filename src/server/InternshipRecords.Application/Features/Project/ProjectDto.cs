using InternshipRecords.Application.Features.Intern;

namespace InternshipRecords.Application.Features.Project;

public class ProjectDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public ICollection<InternDto> Interns { get; set; } = new List<InternDto>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}