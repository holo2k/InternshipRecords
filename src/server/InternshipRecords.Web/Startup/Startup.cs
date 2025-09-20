using System.Text.Json.Serialization;
using FluentValidation;
using InternshipRecords.Application.AutoMapper;
using InternshipRecords.Application.Features.Intern.AddIntern;
using InternshipRecords.Application.Features.Intern.GetInterns;
using InternshipRecords.Infrastructure;
using InternshipRecords.Web.Hub;
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

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetInternsQueryHandler).Assembly));

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<AddInternCommandValidator>();

        services.AddEndpointsApiExplorer();

        services.AddSignalR();

        services.AddCors();
    }

    public static async Task ConfigureAppAsync(WebApplication app)
    {
        await app.MigrateDatabaseAsync();

        app.UseRouting();

        app.UseCors("AllowClient");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<InternsHub>("/internHub");
        });

        app.AddExceptionHandler();

        app.AddSwagger();
    }
}