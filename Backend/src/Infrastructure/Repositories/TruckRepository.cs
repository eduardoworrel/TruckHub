using System;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class TruckRepository(ApplicationDbContext context)
    : GenericRepository<Truck>(context),
        ITruckRepository
{
    public async Task<List<TrucksResponse>> GetAllAsync() =>
        await DbContext
            .Trucks.OrderByDescending(e => e.CreatedAt)
            .Select(e => e.ToResponse())
            .ToListAsync();

    public async Task<List<Truck>> GetOldAsync(int time) =>
        await DbContext
            .Trucks.Where(truck => truck.CreatedAt >= DateTime.Now.AddHours(time))
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

    public async Task<Truck?> GetByIdAsync(Guid id) => await DbContext.Trucks.FindAsync(id);

    public async Task<int> GetCountAsync() => await DbContext.Trucks.CountAsync();

    public async Task AddRangeAsync(List<Truck> trucks)
    {
        await DbContext.Trucks.AddRangeAsync(trucks);
        await DbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Truck truck)
    {
        await DbContext.Trucks.AddAsync(truck);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Truck truck)
    {
        DbContext.Trucks.Update(truck);
        await DbContext.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(List<Guid> ids)
    {
        await DbContext.Trucks.Where(e => ids.Contains(e.Id)).ExecuteDeleteAsync();
    }
}
