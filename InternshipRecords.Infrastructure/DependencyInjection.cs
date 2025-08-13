using InternshipRecords.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InternshipRecords.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connString));

        return services;
    }
}