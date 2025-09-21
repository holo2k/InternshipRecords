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


    public async Task<List<ProjectDto>> GetProjectsAsync(params string[]? queryParams)
    {
        var url = "api/project";
        if (queryParams is { Length: > 0 })
        {
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;
        }

        var response = await _http.GetAsync(url);
        var mb = await MbResultReader.ReadMbResultAsync<List<ProjectDto>>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new List<ProjectDto>();

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<ProjectDto?> AddProjectAsync(AddProjectRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/project", request, _options);
        var mb = await MbResultReader.ReadMbResultAsync<ProjectDto>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new ProjectDto();

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<ProjectDto?> UpdateProjectAsync(UpdateProjectRequest request)
    {
        var response = await _http.PatchAsJsonAsync("api/project", request, _options);
        var mb = await MbResultReader.ReadMbResultAsync<ProjectDto>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new ProjectDto();

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }


    public async Task<Guid?> DeleteProjectAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/project/{id}");
        var mb = await MbResultReader.ReadMbResultAsync<Guid>(response, _options);

        if (mb.IsSuccess) return mb.Result;

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }
}