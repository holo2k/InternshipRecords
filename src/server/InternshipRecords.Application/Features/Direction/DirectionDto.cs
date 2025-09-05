namespace InternshipRecords.Application.Features.Direction;

public class DirectionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public ICollection<Domain.Entities.Intern> Interns { get; set; } = new List<Domain.Entities.Intern>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}