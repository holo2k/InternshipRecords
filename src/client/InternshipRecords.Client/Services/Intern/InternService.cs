using System.Net.Http.Json;
using Shared.Models.Intern;

namespace InternshipRecords.Client.Services.Intern;

public class InternService
{
    private readonly HttpClient _http;

    public InternService(HttpClient http)
    {
        _http = http;
    }

    public async Task<InternDto> GetInternAsync(Guid internId)
    {
        try
        {
            var url = $"api/intern/{internId}";
            return await _http.GetFromJsonAsync<InternDto>(url) ?? new InternDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке стажеров: {ex.Message}");
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
            return await _http.GetFromJsonAsync<List<InternDto>>(url) ?? new List<InternDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке стажеров: {ex.Message}");
            return new List<InternDto>();
        }
    }

    public async Task<InternDto> AddInternAsync(AddInternRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/intern", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<InternDto>();
    }

    public async Task DeleteInternAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/intern/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<InternDto?> UpdateInternAsync(UpdateInternRequest request)
    {
        var msg = new HttpRequestMessage(HttpMethod.Patch, "api/intern")
            { Content = JsonContent.Create(request) };
        var res = await _http.SendAsync(msg);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<InternDto>()!;
    }
}