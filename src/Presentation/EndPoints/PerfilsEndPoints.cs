using Application.Logic;
using Domain.Contracts;
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
        group.MapGet("{id}", GetById);
        group.MapPost("AllByIds", GetAllByIds);
    }

    public static async Task<IResult> Add(
        [FromServices] ITruckBusinessLogic _logic,
        [FromBody] CreateTruckRequest _request
    )
    {
        try
        {
            await _logic.AddTruck(_request);
            return Results.Ok();
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
            await _logic.UpdateTruck(_request);
            return Results.Ok();
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

    public static async Task<IResult> GetAllByIds(
        [FromServices] ITruckBusinessLogic _logic,
        [FromBody] List<Guid> ids
    )
    {
        try
        {
            return Results.Ok(await _logic.GetByIds(ids));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

}
