namespace InternshipRecords.Application.Features.Project.UpdateProject;

public class UpdateProjectRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
}