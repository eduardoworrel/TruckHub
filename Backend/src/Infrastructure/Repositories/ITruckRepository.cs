using Domain.Contracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface ITruckRepository
    {
        public Task<List<TrucksResponse>> GetAllAsync();
        public Task<List<Truck>> GetOldAsync(int time);
        public Task<Truck?> GetByIdAsync(Guid id);
        public Task<int> GetCountAsync();
        public Task AddAsync(Truck truck);
        public Task AddRangeAsync(List<Truck> trucks);
        public Task UpdateAsync(Truck truck);
        public Task RemoveRangeAsync(List<Guid> ids);
    }
}
