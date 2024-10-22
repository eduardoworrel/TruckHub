using Application.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITruckBusinessLogic, TruckBusinessLogic>();
        return services;
    }
}
