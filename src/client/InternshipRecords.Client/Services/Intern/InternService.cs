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
        var url = $"api/intern/{internId}";
        var response = await _http.GetAsync(url);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<InternDto>(_options) ?? new InternDto();

        var serverMessage = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(serverMessage);
    }

    public async Task<List<InternDto>> GetInternsAsync(Guid? directionId = null, Guid? projectId = null)
    {
        var qs = new List<string>();
        if (directionId.HasValue) qs.Add($"directionId={directionId}");
        if (projectId.HasValue) qs.Add($"projectId={projectId}");
        var url = "api/intern" + (qs.Any() ? "?" + string.Join("&", qs) : "");

        var response = await _http.GetAsync(url);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<InternDto>>(_options) ?? new List<InternDto>();

        var serverMessage = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(serverMessage);
    }

    public async Task<InternDto?> AddInternAsync(AddInternRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/intern", request, _options);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<InternDto>(_options);

        var serverMessage = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(serverMessage);
    }

    public async Task DeleteInternAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/intern/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var serverMessage = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(serverMessage);
        }
    }

    public async Task<InternDto?> UpdateInternAsync(UpdateInternRequest request)
    {
        var msg = new HttpRequestMessage(HttpMethod.Patch, "api/intern")
        {
            Content = JsonContent.Create(request, options: _options)
        };

        var response = await _http.SendAsync(msg);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<InternDto>(_options);

        var serverMessage = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(serverMessage);
    }
}