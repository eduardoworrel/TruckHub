using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Logic;

public interface ITruckBusinessLogic
{
    Task<List<TrucksResponse>> GetAll();
    Task<TrucksResponse> GetById(Guid id);
    Task<DashboardInfoResponse> GetDashboardInfo();
    Task<TrucksResponse> AddTruck(CreateTruckRequest createTruckRequest);
    Task<TrucksResponse> UpdateTruck(UpdateTruckRequest updateTruckRequest);
    Task DeleteTrucks(List<Guid> ids);
    Task<List<TrucksResponse>> GenerateAndAdd100Trucks();
}
