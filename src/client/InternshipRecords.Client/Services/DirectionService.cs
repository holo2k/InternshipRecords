using System.Net.Http.Json;
using Shared.Models.Direction;

namespace InternshipRecords.Client.Services;

public class DirectionService
{
    private readonly HttpClient _http;

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
                return await _http.GetFromJsonAsync<List<DirectionDto>>(url) ?? new List<DirectionDto>();
            var queryString = string.Join("&", queryParams.Select(p => $"queryParams={Uri.EscapeDataString(p)}"));
            url += "?" + queryString;

            return await _http.GetFromJsonAsync<List<DirectionDto>>(url) ?? new List<DirectionDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке направлений: {ex.Message}");
            return new List<DirectionDto>();
        }
    }


    public async Task<DirectionDto> AddDirectionAsync(AddDirectionRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/direction", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DirectionDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении направления: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteDirectionAsync(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/direction/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении направления: {ex.Message}");
            return false;
        }
    }
}