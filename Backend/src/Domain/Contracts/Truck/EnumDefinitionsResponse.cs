namespace Domain.Contracts;

public class EnumDefinitionsResponse
{
    public IEnumerable<dynamic> TruckModels { get; set; } = [];
    public IEnumerable<dynamic> PlantLocations { get; set; } = [];
}
