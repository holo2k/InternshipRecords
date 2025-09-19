using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        try
        {
            var url = $"api/intern/{internId}";
            return await _http.GetFromJsonAsync<InternDto>(url, _options) ?? new InternDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке стажера: {ex.Message}");
            return new InternDto();
        }
    }

    public async Task<List<InternDto>> GetInternsAsync(Guid? directionId = null, Guid? projectId = null)
    {
        try
        {
            var qs = new List<string>();
            if (directionId.HasValue) qs.Add($"directionId={directionId}");
            if (projectId.HasValue) qs.Add($"projectId={projectId}");
            var url = "api/intern" + (qs.Any() ? "?" + string.Join("&", qs) : "");
            return await _http.GetFromJsonAsync<List<InternDto>>(url, _options) ?? new List<InternDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке стажеров: {ex.Message}");
            return new List<InternDto>();
        }
    }

    public async Task<InternDto?> AddInternAsync(AddInternRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/intern", request, _options);
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<InternDto>(_options);
        var serverMessage = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(serverMessage);
    }

    public async Task DeleteInternAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/intern/{id}");
    }

    public async Task<InternDto?> UpdateInternAsync(UpdateInternRequest request)
    {
        var msg = new HttpRequestMessage(HttpMethod.Patch, "api/intern")
        {
            Content = JsonContent.Create(request, options: _options)
        };
        var response = await _http.SendAsync(msg);
        return await response.Content.ReadFromJsonAsync<InternDto>(_options);
    }
}