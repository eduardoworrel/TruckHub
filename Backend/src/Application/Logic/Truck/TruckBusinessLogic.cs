using Domain.Contracts;
using Domain.Entities;
using Domain.Enumerables;
using Infrastructure.Repositories;

namespace Application.Logic;

public sealed class TruckBusinessLogic(ITruckRepository _repository) : ITruckBusinessLogic
{
    private const int SIX_HOUR = -6;
    private const int ONE_HOUR = -1;
public async Task<DashboardInfoResponse> GetDashboardInfo()
{
    var total = await _repository.GetCountAsync();
    var trucksLast6Hours = await _repository.GetOldAsync(SIX_HOUR);
    var trucksLast1Hour = await _repository.GetOldAsync(ONE_HOUR);

    var plantCounts = trucksLast6Hours
        .GroupBy(truck => truck.Plant)
        .Select(group => new PlantCount(group.Key.GetDescription(), group.Count()))
        .ToList();

    var minuteCounts = trucksLast1Hour
        .GroupBy(truck => truck.CreatedAt.ToString("HH:mm"))
        .Select(group => new HourCount(
            group.Key,
            group.Count()
        ))
        .ToList();

    var detailedHourCounts = trucksLast6Hours
        .GroupBy(truck => new
        {
            Time = new DateTime(
                truck.CreatedAt.Year,
                truck.CreatedAt.Month,
                truck.CreatedAt.Day,
                truck.CreatedAt.Hour,  // Agrupando por hora completa
                0,  // Minutos zerados para considerar apenas a hora cheia
                0
            ),
            truck.Model,
        })
        .Select(group => new DetailedHourCount(
            group.Key.Time.ToString("HH:mm"),
            group.Key.Model.GetDescription(),
            group.Count()
        ))
        .ToList();

    return new DashboardInfoResponse(
        Total: total,
        PlantCounts: plantCounts,
        HourCounts: minuteCounts,
        DetailedHourCounts: detailedHourCounts
    );
}

    public async Task<List<TrucksResponse>> GetAll()
    {
        var trucks = await _repository.GetAllAsync();

        return trucks;
    }

    public async Task<TrucksResponse> GetById(Guid id)
    {
        var truck =
            await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Truck não encontrado");
        return truck.ToResponse();
    }

    public async Task<TrucksResponse> AddTruck(CreateTruckRequest createTruckRequest)
    {
        var truck = Truck.Create(createTruckRequest);

        await _repository.AddAsync(truck);
        return truck.ToResponse();
    }

    public async Task<TrucksResponse> UpdateTruck(UpdateTruckRequest updateTruckRequest)
    {
        var truck =
            await _repository.GetByIdAsync(updateTruckRequest.Id)
            ?? throw new KeyNotFoundException("Truck não encontrado");

        truck.Update(updateTruckRequest);

        await _repository.UpdateAsync(truck);
        return truck.ToResponse();
    }

    public async Task DeleteTrucks(List<Guid> ids)
    {
        await _repository.RemoveRangeAsync(ids);
    }

    public async Task<List<TrucksResponse>> GenerateAndAdd100Trucks()
    {
        var fakeTrucks = new List<Truck>();

        var faker = new Bogus.Faker<CreateTruckRequest>()
            .RuleFor(r => r.Model, f => f.PickRandom<TruckModel>())
            .RuleFor(r => r.ManufacturingYear, f => f.Date.Past(10).Year)
            .RuleFor(r => r.ChassisCode, f => f.Random.AlphaNumeric(17).ToUpper())
            .RuleFor(r => r.Color, f => f.Commerce.Color())
            .RuleFor(r => r.PlantIsoCode, f => f.PickRandom<PlantLocation>());

        Random rnd = new();
        var requests = faker.Generate(rnd.Next(25000, 100000));
        foreach (var truckRequest in requests)
        {
            var truck = Truck.Create(truckRequest);
            fakeTrucks.Add(truck);
        }

        await _repository.AddRangeAsync(fakeTrucks);

        return fakeTrucks.Select(t => t.ToResponse()).ToList();
    }
}
