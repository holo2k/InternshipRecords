namespace InternshipRecords.Client.Services;

public class ProjectService
{
    private readonly List<string> _projects = new() { "Project A", "Project B" };

    public Task<List<string>> GetProjectsAsync()
    {
        return Task.FromResult(_projects);
    }

    public Task AddProjectAsync(string name)
    {
        if (!_projects.Contains(name)) _projects.Add(name);
        return Task.CompletedTask;
    }
}