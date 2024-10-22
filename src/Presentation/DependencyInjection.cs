using System.Globalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Presentation.EndPoints;

namespace Presentation;

public static class DependencyInjection
{
    /// <summary>
    /// ensure the same behavior in the cloud and locally.
    /// </summary>
    public static IServiceCollection AddSpecificTimeZone(this IServiceCollection services)
    {
        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        return services;
    }

    public static IEndpointRouteBuilder AddEndPoints(this IEndpointRouteBuilder app)
    {
        app.AddTrucksEndPoints();
        return app;
    }
}
