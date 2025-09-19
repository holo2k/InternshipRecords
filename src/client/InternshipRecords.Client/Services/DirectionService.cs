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

    public async Task<List<DirectionDto>> GetDirectionsAsync(params string[]? queryParams)
    {
        try
        {
            var url = "api/direction";

            if (queryParams is not { Length: > 0 })
                return (await _http.GetFromJsonAsync<List<DirectionDto>>(url))!;
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;

            return (await _http.GetFromJsonAsync<List<DirectionDto>>(url, _options))!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке направлений: {ex.Message}");
            return new List<DirectionDto>();
        }
    }


    public async Task<DirectionDto?> UpdateDirectionAsync(UpdateDirectionRequest request)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync("api/direction", request);
            return await response.Content.ReadFromJsonAsync<DirectionDto>(_options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении направления: {ex.Message}");
            return null;
        }
    }

    public async Task<DirectionDto?> AddDirectionAsync(AddDirectionRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/direction", request);
            return await response.Content.ReadFromJsonAsync<DirectionDto>(_options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении направления: {ex.Message}");
            return null;
        }
    }

    public async Task<Guid> DeleteDirectionAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/direction/{id}");
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении направления: {ex.Message}");
            return Guid.Empty;
        }
    }
}