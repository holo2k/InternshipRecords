namespace InternshipRecords.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Startup.Startup.ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        await Startup.Startup.ConfigureAppAsync(app);

        await app.RunAsync();
    }
}