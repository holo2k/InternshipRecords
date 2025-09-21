// DirectionService.cs

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


    public async Task<(List<DirectionDto> Directions, string? Error)> GetDirectionsAsync(params string[]? queryParams)
    {
        try
        {
            var url = "api/direction";
            if (queryParams is { Length: > 0 })
            {
                var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
                url += "?" + queryString;
            }

            var response = await _http.GetAsync(url);
            var mb = await MbResultReader.ReadMbResultAsync<List<DirectionDto>>(response, _options);

            return mb.IsSuccess
                ? (mb.Result ?? new List<DirectionDto>(), null)
                : (new List<DirectionDto>(), mb.Error?.Message);
        }
        catch (Exception ex)
        {
            return (new List<DirectionDto>(), ex.Message);
        }
    }

    public async Task<(DirectionDto? Direction, string? Error)> AddDirectionAsync(AddDirectionRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/direction", request, _options);
            var mb = await MbResultReader.ReadMbResultAsync<DirectionDto>(response, _options);

            if (mb.IsSuccess) return (mb.Result, null);
            return (null, mb.Error?.Message);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(DirectionDto? Direction, string? Error)> UpdateDirectionAsync(UpdateDirectionRequest request)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("api/direction", request, _options);
            var mb = await MbResultReader.ReadMbResultAsync<DirectionDto>(response, _options);

            if (mb.IsSuccess) return (mb.Result, null);
            return (null, mb.Error?.Message);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(Guid? DeletedId, string? Error)> DeleteDirectionAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/direction/{id}");
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