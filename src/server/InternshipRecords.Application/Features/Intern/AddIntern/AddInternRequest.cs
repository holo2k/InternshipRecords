using InternshipRecords.Domain.Enums;

namespace InternshipRecords.Application.Features.Intern.AddIntern;

public class AddInternRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public Guid? DirectionId { get; set; }
    public Guid? ProjectId { get; set; }
}