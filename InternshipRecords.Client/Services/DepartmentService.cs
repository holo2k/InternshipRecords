namespace InternshipRecords.Client.Services;

public class DepartmentService
{
    private readonly List<string> _departments = new() { "Development", "QA", "HR" };

    public Task<List<string>> GetDepartmentsAsync()
    {
        return Task.FromResult(_departments);
    }

    public Task AddDepartmentAsync(string name)
    {
        if (!_departments.Contains(name)) _departments.Add(name);
        return Task.CompletedTask;
    }
}