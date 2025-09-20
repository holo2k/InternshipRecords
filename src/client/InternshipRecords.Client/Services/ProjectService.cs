using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Client.Services;

public class ProjectService
{
    private readonly HttpClient _http;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public ProjectService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProjectDto?>> GetProjectsAsync(params string[]? queryParams)
    {
        try
        {
            var url = "api/project";

            if (queryParams is not { Length: > 0 })
                return (await _http.GetFromJsonAsync<List<ProjectDto>>(url))!;
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;

            return (await _http.GetFromJsonAsync<List<ProjectDto>>(url, _options))!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке направлений: {ex.Message}");
            return new List<ProjectDto?>();
        }
    }

    public async Task<ProjectDto?> AddProjectAsync(AddProjectRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/project", request);
            return await response.Content.ReadFromJsonAsync<ProjectDto>(_options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении проекта: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectDto?> UpdateProjectAsync(UpdateProjectRequest request)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("api/project", request);
            return await response.Content.ReadFromJsonAsync<ProjectDto>(_options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении проекта: {ex.Message}");
            return null;
        }
    }

    public async Task<Guid?> DeleteProjectAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/project/{id}");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Guid>(_options);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>(_options);
        Console.WriteLine($"Ошибка удаления: {error?["message"]}");
        return null;
    }
}