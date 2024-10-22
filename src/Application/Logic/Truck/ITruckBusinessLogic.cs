using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Logic;

public interface ITruckBusinessLogic
{
    Task<List<Truck>> GetAll();
    Task<Truck> GetById(Guid id);
    Task<List<Truck>> GetByIds(List<Guid> ids);
    Task AddTruck(CreateTruckRequest createTruckRequest);
    Task UpdateTruck(UpdateTruckRequest updateTruckRequest);
}
