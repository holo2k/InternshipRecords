using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;

rn;

namespace InternshipRecords.Client.Services.Intern;

public class InternSignalRService : IAsyncDisposable
{
    private readonly HubConnection _hub;

    public InternSignalRService(HubConnection hubConnection)
    {
        _hub = hubConnection;

        _hub.On<InternDto>("InternCreated", dto => InternCreated?.Invoke(dto));
        _hub.On<InternDto>("InternUpdated", dto => InternUpdated?.Invoke(dto));
        _hub.On<Guid>("InternDeleted", id => InternDeleted?.Invoke(id));
    }

    public ValueTask DisposeAsync()
    {
        return _hub.DisposeAsync();
    }

    public event Action<InternDto>? InternCreated;
    public event Action<InternDto>? InternUpdated;
    public event Action<Guid>? InternDeleted;

    public async Task StartAsync()
    {
        if (_hub.State == HubConnectionState.Disconnected)
            await _hub.StartAsync();
    }

    public async Task StopAsync()
    {
        if (_hub.State != HubConnectionState.Disconnected)
            await _hub.StopAsync();
    }
}