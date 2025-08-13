using InternshipRecords.Client;
using InternshipRecords.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5001/")
});

builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<InternService>();
builder.Services.AddSingleton<DepartmentService>();
builder.Services.AddSingleton<ProjectService>();


builder.Services.AddSingleton(sp =>
{
    var connection = new HubConnectionBuilder()
        .WithUrl("http://localhost:5001/chatHub")
        .WithAutomaticReconnect()
        .Build();
    return connection;
});

await builder.Build().RunAsync();