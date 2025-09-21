using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InternshipRecords.Client.Helpers;
using Shared.Models.Direction;
using Shared.Models.Project;

namespace InternshipRecords.Client.Services;

public class DirectionService
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

    public DirectionService(HttpClient http)
    {
        _http = http;
    }


    public async Task<List<DirectionDto>> GetDirectionsAsync(params string[]? queryParams)
    {
        var url = "api/direction";
        if (queryParams is { Length: > 0 })
        {
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;
        }

        var response = await _http.GetAsync(url);
        var mb = await MbResultReader.ReadMbResultAsync<List<DirectionDto>>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new List<DirectionDto>();
        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<DirectionDto?> AddDirectionAsync(AddDirectionRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/direction", request, _options);
        var mb = await MbResultReader.ReadMbResultAsync<DirectionDto>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new DirectionDto();
        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<DirectionDto?> UpdateDirectionAsync(UpdateDirectionRequest request)
    {
        var response = await _http.PatchAsJsonAsync("api/direction", request, _options);
        var mb = await MbResultReader.ReadMbResultAsync<DirectionDto>(response, _options);

        if (mb.IsSuccess) return mb.Result ?? new DirectionDto();
        var validationErrors = mb.Error!.ValidationErrors?
            .Values
            .SelectMany(v => v)
            .ToArray() ?? Array.Empty<string>();

        throw new InvalidOperationException(
            $"{mb.Error.Message}{(validationErrors.Any() ? " : " + string.Join(" ", validationErrors) : string.Empty)}"
        );
    }

    public async Task<Guid?> DeleteDirectionAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/direction/{id}");
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