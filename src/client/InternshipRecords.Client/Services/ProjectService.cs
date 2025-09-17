using System.Net.Http.Json;
using Shared.Models;
using Shared.Models.Project;

namespace InternshipRecords.Client.Services;

public class ProjectService
{
    private readonly HttpClient _http;

    public ProjectService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProjectDto>> GetProjectsAsync(params string[]? queryParams)
    {
        try
        {
            var url = "api/project";

            if (queryParams is not { Length: > 0 })
                return await _http.GetFromJsonAsync<List<ProjectDto>>(url) ?? new List<ProjectDto>();
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;

            return await _http.GetFromJsonAsync<List<ProjectDto>>(url) ?? new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке направлений: {ex.Message}");
            return new List<ProjectDto>();
        }
    }

    public async Task<ProjectDto> AddProjectAsync(AddProjectRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/project", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении проекта: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/project/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении проекта: {ex.Message}");
            return false;
        }
    }
}