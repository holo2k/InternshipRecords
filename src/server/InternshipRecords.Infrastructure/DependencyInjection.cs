using InternshipRecords.Infrastructure.Persistence;
using InternshipRecords.Infrastructure.Repository.Abstractions;
using InternshipRecords.Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InternshipRecords.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connString));

        services.AddScoped<IDirectionRepository, DirectionRepository>();
        services.AddScoped<IInternRepository, InternRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        return services;
    }
}