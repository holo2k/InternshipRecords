using InternshipRecords.Application.Features.Direction;
using InternshipRecords.Application.Features.Project;
using InternshipRecords.Domain.Enums;

namespace InternshipRecords.Application.Features.Intern;

public class InternDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public Guid? DirectionId { get; set; }
    public DirectionDto? Direction { get; set; } = null!;

    public Guid? ProjectId { get; set; }
    public ProjectDto? Project { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}