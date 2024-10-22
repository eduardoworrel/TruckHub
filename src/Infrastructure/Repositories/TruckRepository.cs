using System;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class TruckRepository(ApplicationDbContext context)
    : GenericRepository<Truck>(context),
        ITruckRepository
{
    public async Task<List<Truck>> GetAllAsync() => await DbContext.Trucks.ToListAsync();

    public async Task<Truck?> GetByIdAsync(Guid id) => await DbContext.Trucks.FindAsync(id);

    public async Task<List<Truck>> GetByIdsAsync(List<Guid> ids) =>
        await DbContext.Trucks.Where(e => ids.Contains(e.Id)).ToListAsync();

    public async Task AddAsync(Truck Truck)
    {
        await DbContext.Trucks.AddAsync(Truck);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Truck Truck)
    {
        DbContext.Trucks.Update(Truck);
        await DbContext.SaveChangesAsync();
    }
}
