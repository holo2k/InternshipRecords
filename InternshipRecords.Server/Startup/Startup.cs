using System.Text.Json.Serialization;
using InternshipRecords.Infrastructure;
using InternshipRecords.Server.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Server.Startup;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        var connection = configuration.GetConnectionString("DefaultConnection");

        Console.WriteLine(connection);

        services.AddInfrastructure(connection!);

        services.AddSwagger(configuration);

        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddSignalR();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowClient", builder =>
            {
                builder.WithOrigins("http://localhost:5000") // порт клиента
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static async Task ConfigureAppAsync(WebApplication app)
    {
        await app.MigrateDatabaseAsync();

        app.UseCors("AllowClient");

        app.AddExceptionHandler();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ChatHub>("/chatHub"); // обязательно именно MapHub
        });

        app.AddSwagger();

        app.MapControllers();
    }
}