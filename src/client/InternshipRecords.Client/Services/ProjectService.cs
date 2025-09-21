// ProjectService.cs

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InternshipRecords.Client.Helpers;
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
            var mb = await MbResultReader.ReadMbResultAsync<List<ProjectDto>>(response, _options);

            return mb.IsSuccess
                ? (mb.Result ?? new List<ProjectDto>(), null)
                : (new List<ProjectDto>(), mb.Error?.Message);
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
            var response = await _http.PostAsJsonAsync("api/project", request, _options);
            var mb = await MbResultReader.ReadMbResultAsync<ProjectDto>(response, _options);

            if (mb.IsSuccess) return (mb.Result, null);
            return (null, mb.Error?.Message);
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
            var response = await _http.PatchAsJsonAsync("api/project", request, _options);
            var mb = await MbResultReader.ReadMbResultAsync<ProjectDto>(response, _options);

            if (mb.IsSuccess) return (mb.Result, null);
            return (null, mb.Error?.Message);
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
            var mb = await MbResultReader.ReadMbResultAsync<Guid>(response, _options);

            if (mb.IsSuccess) return (mb.Result, null);
            return (null, mb.Error?.Message);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}