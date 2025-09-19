namespace Shared.Models.Project;

public class UpdateDirectionRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
    public List<Guid>? InternIds { get; set; } = new();
}