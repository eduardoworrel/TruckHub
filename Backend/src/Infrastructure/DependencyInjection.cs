using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString =
            Environment.GetEnvironmentVariable("APP_CONNECTIONSTRING")
            ?? configuration.GetConnectionString("APP_CONNECTIONSTRING")
            ?? throw new KeyNotFoundException("APP_CONNECTIONSTRING");

        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseSqlite(connectionString, b => b.MigrationsAssembly("Infrastructure"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITruckRepository, TruckRepository>();

        return services;
    }
}
