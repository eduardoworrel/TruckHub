namespace Domain.Contracts;

public record DashboardInfoResponse(
    int Total,
    List<PlantCount> PlantCounts,
    List<HourCount> HourCounts,
    List<DetailedHourCount> DetailedHourCounts
);

public record DetailedHourCount(string Time, string ModelName, int Count);

public record HourCount(string Time, int Count);

public record PlantCount(string Country, int Count);
