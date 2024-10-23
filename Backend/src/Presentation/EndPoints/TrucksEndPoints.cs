using Application.Logic;
using Domain.Contracts;
using Domain.Enumerables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.EndPoints;

public static class TrucksEndPoints
{
    public static void AddTrucksEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/trucks");

        group.WithOpenApi();

        group.MapGet(string.Empty, GetAll);
        group.MapPost(string.Empty, Add);
        group.MapPut(string.Empty, Update);
        group.MapDelete(string.Empty, DeleteRange);
        group.MapGet("{id}", GetById);
        group.MapGet("definitions", GetEnumDefinitions);
        group.MapGet("dashboard", FillDashboard);
        group.MapGet("generate", Generate100);
    }

    public static async Task<IResult> FillDashboard([FromServices] ITruckBusinessLogic _logic)
    {
        try
        {
            return Results.Ok(await _logic.GetDashboardInfo());
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> Generate100([FromServices] ITruckBusinessLogic _logic)
    {
        try
        {
            return Results.Ok(await _logic.GenerateAndAdd100Trucks());
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteRange(
        [FromServices] ITruckBusinessLogic _logic,
        [FromBody] List<Guid> ids
    )
    {
        try
        {
            await _logic.DeleteTrucks(ids);
            return Results.Ok("Trucks deleted successfully.");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> Add(
        [FromServices] ITruckBusinessLogic _logic,
        [FromBody] CreateTruckRequest _request
    )
    {
        try
        {
            return Results.Ok(await _logic.AddTruck(_request));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> Update(
        [FromServices] ITruckBusinessLogic _logic,
        [FromBody] UpdateTruckRequest _request
    )
    {
        try
        {
            Console.WriteLine("oi");
            return Results.Ok(await _logic.UpdateTruck(_request));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> GetAll([FromServices] ITruckBusinessLogic _logic)
    {
        try
        {
            return Results.Ok(await _logic.GetAll());
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> GetById([FromServices] ITruckBusinessLogic _logic, Guid id)
    {
        try
        {
            return Results.Ok(await _logic.GetById(id));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static IResult GetEnumDefinitions()
    {
        var truckModels = Enum.GetValues(typeof(TruckModel))
            .Cast<TruckModel>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString(),
                Description = e.GetDescription(),
            });

        var plantLocations = Enum.GetValues(typeof(PlantLocation))
            .Cast<PlantLocation>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString(),
                Description = e.GetDescription(),
            });

        return Results.Json(new { TruckModels = truckModels, PlantLocations = plantLocations });
    }
}
