using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var list = JsonSerializer.Deserialize<List<DirectionDto>>(content, _options) ??
                           new List<DirectionDto>();
                return (list, null);
            }

            var error = JsonSerializer.Deserialize<Dictionary<string, object>>(content, _options);
            return (new List<DirectionDto>(), error?["message"].ToString());
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
            var response = await _http.PostAsJsonAsync("api/direction", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<DirectionDto>(content, _options);
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

    public async Task<(DirectionDto? Direction, string? Error)> UpdateDirectionAsync(UpdateDirectionRequest request)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("api/direction", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<DirectionDto>(content, _options);
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

    public async Task<(Guid? DeletedId, string? Error)> DeleteDirectionAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/direction/{id}");
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