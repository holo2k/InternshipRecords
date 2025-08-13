using System.Reflection;
using Microsoft.OpenApi.Models;

namespace InternshipRecords.Server.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "InternshipRecords API",
                Version = "v1",
                Description = "API для управления веб-приложением \"Учёт стажёров\""
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowClient", builder =>
            {
                builder.WithOrigins("https://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}