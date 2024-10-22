using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Logic;

public sealed class TruckBusinessLogic(ITruckRepository _repository) : ITruckBusinessLogic
{
    public async Task<List<Truck>> GetAll() => await _repository.GetAllAsync();

    public async Task<Truck> GetById(Guid id) =>
        await _repository.GetByIdAsync(id)
        ?? throw new KeyNotFoundException("Truck não encontrado");

    public async Task<List<Truck>> GetByIds(List<Guid> ids) =>
        await _repository.GetByIdsAsync(ids)
        ?? throw new KeyNotFoundException("Truck não encontrado");

    public async Task AddTruck(CreateTruckRequest createTruckRequest)
    {
        var truck = Truck.Create(createTruckRequest);

        await _repository.AddAsync(truck);
    }

    public async Task UpdateTruck(UpdateTruckRequest updateTruckRequest)
    {
        var truck =
            await _repository.GetByIdAsync(updateTruckRequest.Id)
            ?? throw new KeyNotFoundException("Truck não encontrado");

        truck.Update(updateTruckRequest);

        await _repository.UpdateAsync(truck);
    }
}
