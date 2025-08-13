namespace InternshipRecords.Client.Services;

public class InternService
{
    private readonly List<InternDto> _interns = new();

    public Task<List<InternDto>> GetInternsAsync()
    {
        return Task.FromResult(_interns);
    }

    public Task AddInternAsync(InternDto intern)
    {
        intern.Id = _interns.Count + 1;
        _interns.Add(intern);
        return Task.CompletedTask;
    }

    public Task UpdateInternAsync(InternDto intern)
    {
        var idx = _interns.FindIndex(i => i.Id == intern.Id);
        if (idx >= 0) _interns[idx] = intern;
        return Task.CompletedTask;
    }
}

public class InternDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public string Department { get; set; } = "";
    public string Project { get; set; } = "";
}