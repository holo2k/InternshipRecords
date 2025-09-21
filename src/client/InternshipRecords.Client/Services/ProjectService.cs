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

    public async Task<(List<ProjectDto> Projects, string? Error)> GetProjectsAsync(params string[]? queryParams)
    {
        try
        {
            var url = "api/project";
            if (queryParams is { Length: > 0 })
            {
                var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
                url += "?" + queryString;
            }

            var response = await _http.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var list = JsonSerializer.Deserialize<List<ProjectDto>>(content, _options) ?? new List<ProjectDto>();
                return (list, null);
            }

            var error = JsonSerializer.Deserialize<Dictionary<string, object>>(content, _options);
            return (new List<ProjectDto>(), error?["message"].ToString());
        }
        catch (Exception ex)
        {
            return (new List<ProjectDto>(), ex.Message);
        }
    }

    public async Task<(ProjectDto? Project, string? Error)> AddProjectAsync(AddProjectRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/project", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<ProjectDto>(content, _options);
                return (dto, null);
            }

            var error = JsonSerializer.Deserialize<Dictionary<string, object>>(content, _options);
            return (null, error?["message"].ToString());
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(ProjectDto? Project, string? Error)> UpdateProjectAsync(UpdateProjectRequest request)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("api/project", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<ProjectDto>(content, _options);
                return (dto, null);
            }

            var error = JsonSerializer.Deserialize<Dictionary<string, object>>(content, _options);
            return (null, error?["message"].ToString());
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(Guid? DeletedId, string? Error)> DeleteProjectAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/project/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<Guid>(content, _options);
                return (dto, null);
            }

            var error = JsonSerializer.Deserialize<Dictionary<string, object>>(content, _options);
            return (null, error?["message"].ToString());
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}