namespace InternshipRecords.Application.Features.Direction.UpdateDirection;

public class UpdateDirectionRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
}