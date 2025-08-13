using System.Text.Json;
using InternshipRecords.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace InternshipRecords.Server.Startup;

public static class WebAppExtensions
{
    public static void AddExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionFeature?.Error;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorResponse = new
                {
                    code = "InternalServerError",
                    message = exception?.Message ?? "An unexpected error occurred."
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            });
        });
    }

    public static void AddSwagger(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger", true);
                return;
            }

            if (context.RequestAborted.IsCancellationRequested) return;

            await next();
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service API");
            c.RoutePrefix = "swagger";
        });
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();
        await appDbContext.SaveChangesAsync();
    }
}