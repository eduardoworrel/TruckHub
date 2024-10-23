using System.Globalization;
using Domain.Contracts;
using Domain.Enumerables;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Truck(
    Guid id,
    TruckModel model,
    int manufacturingYear,
    string chassisCode,
    string color,
    PlantLocation plant
) : Entity(id)
{
    public TruckModel Model { get; private set; } = model;
    public int ManufacturingYear { get; private set; } = manufacturingYear;
    public string ChassisCode { get; private set; } = chassisCode;
    public string Color { get; private set; } = color;
    public PlantLocation Plant { get; private set; } = plant;

    public static Truck Create(CreateTruckRequest request)
    {
        return new Truck(
            id: Guid.NewGuid(),
            model: request.Model,
            manufacturingYear: request.ManufacturingYear,
            chassisCode: request.ChassisCode,
            color: request.Color,
            plant: request.PlantIsoCode
        );
    }

    public void Update(UpdateTruckRequest request)
    {
        Model = request.Model;
        ManufacturingYear = request.ManufacturingYear;
        ChassisCode = request.ChassisCode;
        Color = request.Color;
        Plant = request.PlantIsoCode;
    }

    public TrucksResponse ToResponse()
    {
        return new TrucksResponse(
            Id,
            Model.GetDescription(),
            ManufacturingYear,
            ChassisCode,
            Color,
            Plant.GetDescription(),
            CreatedAt.ToString("G", CultureInfo.CurrentCulture)
        );
    }
}
