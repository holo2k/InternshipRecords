using System.Text.Json;
using Shared.Models;

namespace InternshipRecords.Client.Helpers;

public static class MbResultReader
{
    public static async Task<MbResult<T>> ReadMbResultAsync<T>(HttpResponseMessage response,
        JsonSerializerOptions options)
    {
        var content = await response.Content.ReadAsStringAsync();
        var mb = JsonSerializer.Deserialize<MbResult<T>>(content, options);
        return mb ?? throw new InvalidOperationException($"Invalid server response: {content}");
    }
}