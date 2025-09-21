// InternService.cs

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InternshipRecords.Client.Helpers;
using Shared.Models.Intern;

namespace InternshipRecords.Client.Services.Intern;

public class InternService
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

    public InternService(HttpClient http)
    {
        _http = http;
    }


    public async Task<InternDto> GetInternAsync(Guid internId)
    {
        var url = $"api/intern/{internId}";
        var response = await _http.GetAsync(url);
        var mb = await MbResultReader.ReadMbResultAsync<InternDto>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new InternDto();
        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<List<InternDto>> GetInternsAsync(Guid? directionId = null, Guid? projectId = null)
    {
        var qs = new List<string>();
        if (directionId.HasValue) qs.Add($"directionId={directionId}");
        if (projectId.HasValue) qs.Add($"projectId={projectId}");
        var url = "api/intern" + (qs.Any() ? "?" + string.Join("&", qs) : "");

        var response = await _http.GetAsync(url);
        var mb = await MbResultReader.ReadMbResultAsync<List<InternDto>>(response, _options);

        if (mb!.IsSuccess) return mb.Result ?? new List<InternDto>();

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<InternDto?> AddInternAsync(AddInternRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/intern", request, _options);

        var mb = await MbResultReader.ReadMbResultAsync<InternDto>(response, _options);

        if (mb.IsSuccess) return mb.Result;

        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task DeleteInternAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/intern/{id}");
        var mb = await MbResultReader.ReadMbResultAsync<Guid?>(response, _options);

        if (mb.IsSuccess) return;
        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<InternDto?> UpdateInternAsync(UpdateInternRequest request)
    {
        var msg = new HttpRequestMessage(HttpMethod.Patch, "api/intern")
        {
            Content = JsonContent.Create(request, options: _options)
        };

        var response = await _http.SendAsync(msg);
        var mb = await MbResultReader.ReadMbResultAsync<InternDto>(response, _options);

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