using Domain.Enumerables;

namespace Domain.Contracts;

public record TrucksResponse(
    Guid Id,
    string Model,
    int ManufacturingYear,
    string ChassisCode,
    string Color,
    string PlantName,
    string CreatedAt
);
