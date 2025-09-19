using Shared.Enums;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace Shared.Models.Intern;

public class InternDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateOnly BirthDate { get; set; }

    public Guid? DirectionId { get; set; }
    public DirectionDto? Direction { get; set; } = null!;

    public Guid? ProjectId { get; set; }
    public ProjectDto? Project { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}