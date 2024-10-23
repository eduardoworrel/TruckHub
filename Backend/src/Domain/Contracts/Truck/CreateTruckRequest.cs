using Domain.Enumerables;

namespace Domain.Contracts;

public record CreateTruckRequest
{
    public TruckModel Model { get; set; }
    public int ManufacturingYear { get; set; }
    public string ChassisCode { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public PlantLocation PlantIsoCode { get; set; }
};
