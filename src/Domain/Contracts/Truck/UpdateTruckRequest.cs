using Domain.Enumerables;

namespace Domain.Contracts;

public record UpdateTruckRequest(
    Guid Id,
    TruckModel Model,
    int ManufacturingYear,
    string ChassisCode,
    string Color,
    PlantLocation PlantIsoCode
);
