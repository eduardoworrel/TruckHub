using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface ITruckRepository
    {
        public Task<List<Truck>> GetAllAsync();
        public Task<Truck?> GetByIdAsync(Guid id);
        public Task<List<Truck>> GetByIdsAsync(List<Guid> ids);
        public Task AddAsync(Truck Truck);
        public Task UpdateAsync(Truck Truck);
    }
}
