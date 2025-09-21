using System.Text.Json;
using InternshipRecords.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace InternshipRecords.Web.Startup;

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

                var result = MbResult<object>.Fail(new MbError("InternalServerError", exception!.Message));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var json = JsonSerializer.Serialize(result);
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