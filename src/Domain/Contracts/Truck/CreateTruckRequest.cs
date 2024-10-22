using Domain.Enumerables;

namespace Domain.Contracts;

public record CreateTruckRequest(
    TruckModel Model,
    int ManufacturingYear,
    string ChassisCode,
    string Color,
    PlantLocation PlantIsoCode
);
