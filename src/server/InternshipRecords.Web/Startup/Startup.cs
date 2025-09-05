using System.Text.Json.Serialization;
using FluentValidation;
using InternshipRecords.Application.AutoMapper;
using InternshipRecords.Infrastructure;
using InternshipRecords.Web.PipelineBehaviors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipRecords.Web.Startup;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        var connection = configuration.GetConnectionString("DefaultConnection");

        services.AddInfrastructure(connection!);

        services.AddSwagger(configuration);

        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        app.AddSwagger();

        app.MapControllers();
    }
}