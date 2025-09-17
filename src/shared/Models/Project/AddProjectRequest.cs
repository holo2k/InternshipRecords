namespace Shared.Models;

public class AddProjectRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
}