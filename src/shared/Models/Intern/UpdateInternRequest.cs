using Shared.Enums;

namespace Shared.Models.Intern;

public class UpdateInternRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateOnly BirthDate { get; set; }
    public Guid? DirectionId { get; set; }
    public Guid? ProjectId { get; set; }
}