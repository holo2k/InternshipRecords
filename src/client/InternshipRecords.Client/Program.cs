using InternshipRecords.Client;
using InternshipRecords.Client.Services;
using InternshipRecords.Client.Services.Intern;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5001/")
});

builder.Services.AddScoped<InternService>();
builder.Services.AddScoped<DirectionService>();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddSingleton<InternSignalRService>();

builder.Services.AddSingleton(sp =>
{
    var connection = new HubConnectionBuilder()
        .WithUrl("http://localhost:5001/internHub")
        .WithAutomaticReconnect()
        .Build();
    return connection;
});

await builder.Build().RunAsync();