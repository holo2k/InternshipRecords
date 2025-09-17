using Shared.Models.Intern;

namespace Shared.Models.Direction;

public class DirectionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public ICollection<InternDto> Interns { get; set; } = new List<InternDto>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}