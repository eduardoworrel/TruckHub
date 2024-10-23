using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enumerables;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace tests
{
    public class TruckRepositoryTests
    {
        private static TruckRepository CreateRepository()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            var context = new ApplicationDbContext(options);
            return new TruckRepository(context);
        }

        private static async Task SeedDatabase(TruckRepository repository)
        {
            var trucks = new List<Truck>
            {
                Truck.Create(
                    new CreateTruckRequest
                    {
                        Model = TruckModel.FH,
                        ManufacturingYear = 2021,
                        ChassisCode = "ABC123",
                        Color = "Red",
                        PlantIsoCode = PlantLocation.BR,
                    }
                ),
                Truck.Create(
                    new CreateTruckRequest
                    {
                        Model = TruckModel.FM,
                        ManufacturingYear = 2022,
                        ChassisCode = "DEF456",
                        Color = "Blue",
                        PlantIsoCode = PlantLocation.US,
                    }
                ),
            };

            await repository.AddRangeAsync(trucks);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTrucksOrderedByCreatedAt()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("DEF456", result.First().ChassisCode); // Ensure it's ordered by CreatedAt descending
        }

        [Fact]
        public async Task GetOldAsync_ShouldReturnTrucksCreatedWithinGivenTime()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            var time = -24; // Últimas 24 horas

            // Act
            var result = await repository.GetOldAsync(time);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTruck_WhenTruckExists()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            var truck = (await repository.GetAllAsync()).First();
            var truckId = truck.Id;

            // Act
            var result = await repository.GetByIdAsync(truckId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(truckId, result?.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTruckDoesNotExist()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCountAsync_ShouldReturnTotalNumberOfTrucks()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            // Act
            var result = await repository.GetCountAsync();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task AddRangeAsync_ShouldAddMultipleTrucks()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            var newTrucks = new List<Truck>
            {
                Truck.Create(
                    new CreateTruckRequest
                    {
                        Model = TruckModel.VM,
                        ManufacturingYear = 2020,
                        ChassisCode = "GHI789",
                        Color = "Green",
                        PlantIsoCode = PlantLocation.FR,
                    }
                ),
                Truck.Create(
                    new CreateTruckRequest
                    {
                        Model = TruckModel.FM,
                        ManufacturingYear = 2019,
                        ChassisCode = "JKL012",
                        Color = "Black",
                        PlantIsoCode = PlantLocation.SE,
                    }
                ),
            };

            // Act
            await repository.AddRangeAsync(newTrucks);

            // Assert
            var count = await repository.GetCountAsync();
            Assert.Equal(4, count); // Previously 2, now 4
        }

        [Fact]
        public async Task AddAsync_ShouldAddSingleTruck()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            var newTruck = Truck.Create(
                new CreateTruckRequest
                {
                    Model = TruckModel.FH,
                    ManufacturingYear = 2023,
                    ChassisCode = "MNO345",
                    Color = "Yellow",
                    PlantIsoCode = PlantLocation.BR,
                }
            );

            // Act
            await repository.AddAsync(newTruck);

            // Assert
            var count = await repository.GetCountAsync();
            Assert.Equal(3, count); // Previously 2, now 3
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTruck()
        {
            // Arrange
            var repository = CreateRepository();
            await SeedDatabase(repository);

            var truckResponse = (await repository.GetAllAsync()).First();
            var truck = await repository.GetByIdAsync(truckResponse.Id) ?? throw new Exception();
            var updatedTruck = new UpdateTruckRequest(
                truck.Id,
                TruckModel.FM,
                2023,
                "UPDATED123",
                "UpdatedColor",
                PlantLocation.US
            );
            truck.Update(updatedTruck);

            // Act
            await repository.UpdateAsync(truck);

            // Assert
            var result = await repository.GetByIdAsync(truck.Id);
            Assert.Equal("UPDATED123", result?.ChassisCode);
            Assert.Equal("UpdatedColor", result?.Color);
        }
    }
}
