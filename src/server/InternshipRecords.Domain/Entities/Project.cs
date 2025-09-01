namespace InternshipRecords.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public ICollection<Intern> Interns { get; set; } = new List<Intern>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}