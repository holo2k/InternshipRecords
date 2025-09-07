namespace InternshipRecords.Application.Features.Project.AddProject;

public class AddProjectRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
}