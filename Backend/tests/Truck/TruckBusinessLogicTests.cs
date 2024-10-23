using Application.Logic;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enumerables;
using Infrastructure.Repositories;
using Moq;

namespace tests
{
    public class TruckBusinessLogicTests
    {
        private readonly Mock<ITruckRepository> mockRepository;
        private readonly TruckBusinessLogic truckLogic;

        public TruckBusinessLogicTests()
        {
            mockRepository = new Mock<ITruckRepository>();
            truckLogic = new TruckBusinessLogic(mockRepository.Object);
        }

        [Fact]
        public async Task GetDashboardInfo_ShouldReturnCorrectDashboardInfo()
        {
            // Arrange
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

            mockRepository.Setup(repo => repo.GetCountAsync()).ReturnsAsync(100);
            mockRepository.Setup(repo => repo.GetOldAsync(It.IsAny<int>())).ReturnsAsync(trucks);

            // Act
            var dashboardInfo = await truckLogic.GetDashboardInfo();

            // Assert
            Assert.Equal(100, dashboardInfo.Total);
            Assert.Equal(2, dashboardInfo.PlantCounts.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfTrucks()
        {
            // Arrange
            var trucks = new List<TrucksResponse>
            {
                Truck
                    .Create(
                        new CreateTruckRequest
                        {
                            Model = TruckModel.FH,
                            ManufacturingYear = 2021,
                            ChassisCode = "ABC123",
                            Color = "Red",
                            PlantIsoCode = PlantLocation.BR,
                        }
                    )
                    .ToResponse(),
                Truck
                    .Create(
                        new CreateTruckRequest
                        {
                            Model = TruckModel.FM,
                            ManufacturingYear = 2022,
                            ChassisCode = "DEF456",
                            Color = "Blue",
                            PlantIsoCode = PlantLocation.US,
                        }
                    )
                    .ToResponse(),
            };

            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(trucks);

            // Act
            var result = await truckLogic.GetAll();

            // Assert
            Assert.Equal(trucks.Count, result.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnTruck_WhenFound()
        {
            // Arrange

            var truck = Truck.Create(
                new CreateTruckRequest
                {
                    Model = TruckModel.FH,
                    ManufacturingYear = 2021,
                    ChassisCode = "ABC123",
                    Color = "Red",
                    PlantIsoCode = PlantLocation.BR,
                }
            );
            var truckId = truck.Id;
            mockRepository.Setup(repo => repo.GetByIdAsync(truckId)).ReturnsAsync(truck);

            // Act
            var result = await truckLogic.GetById(truckId);

            // Assert
            Assert.Equal(truckId, result.Id);
        }

        [Fact]
        public async Task GetById_ShouldThrowException_WhenTruckNotFound()
        {
            // Arrange
            var truckId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetByIdAsync(truckId)).ReturnsAsync((Truck)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => truckLogic.GetById(truckId));
        }

        [Fact]
        public async Task AddTruck_ShouldAddTruckAndReturnResponse()
        {
            // Arrange
            var request = new CreateTruckRequest
            {
                Model = TruckModel.FH,
                ManufacturingYear = 2021,
                ChassisCode = "ABC123",
                Color = "Red",
                PlantIsoCode = PlantLocation.BR,
            };

            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Truck>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await truckLogic.AddTruck(request);

            // Assert
            Assert.Equal(request.Model.GetDescription(), result.Model);
        }

        [Fact]
        public async Task UpdateTruck_ShouldUpdateTruckAndReturnResponse()
        {
            // Arrange
            var truckId = Guid.NewGuid();
            var truck = Truck.Create(
                new CreateTruckRequest
                {
                    Model = TruckModel.FH,
                    ManufacturingYear = 2021,
                    ChassisCode = "ABC123",
                    Color = "Red",
                    PlantIsoCode = PlantLocation.BR,
                }
            );

            var request = new UpdateTruckRequest(
                Id: truckId,
                Model: TruckModel.FM,
                ManufacturingYear: 2022,
                ChassisCode: "DEF456",
                Color: "Blue",
                PlantIsoCode: PlantLocation.US
            );

            mockRepository.Setup(repo => repo.GetByIdAsync(truckId)).ReturnsAsync(truck);
            mockRepository.Setup(repo => repo.UpdateAsync(truck)).Returns(Task.CompletedTask);

            // Act
            var result = await truckLogic.UpdateTruck(request);

            // Assert
            Assert.Equal(request.Model.GetDescription(), result.Model);
        }

        [Fact]
        public async Task UpdateTruck_ShouldThrowException_WhenTruckNotFound()
        {
            // Arrange
            var truckId = Guid.NewGuid();
            var request = new UpdateTruckRequest(
                Id: truckId,
                Model: TruckModel.FM,
                ManufacturingYear: 2022,
                ChassisCode: "DEF456",
                Color: "Blue",
                PlantIsoCode: PlantLocation.US
            );

            mockRepository.Setup(repo => repo.GetByIdAsync(truckId)).ReturnsAsync((Truck)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => truckLogic.UpdateTruck(request));
        }

        [Fact]
        public async Task DeleteTrucks_ShouldDeleteTrucks()
        {
            // Arrange
            var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            mockRepository.Setup(repo => repo.RemoveRangeAsync(ids)).Returns(Task.CompletedTask);

            // Act
            await truckLogic.DeleteTrucks(ids);

            // Assert
            mockRepository.Verify(repo => repo.RemoveRangeAsync(ids), Times.Once);
        }

        [Fact]
        public async Task GenerateAndAdd100Trucks_ShouldGenerateAndAddTrucks()
        {
            // Arrange
            mockRepository
                .Setup(repo => repo.AddRangeAsync(It.IsAny<List<Truck>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await truckLogic.GenerateAndAdd100Trucks();

            // Assert
            Assert.NotEmpty(result);
            mockRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<List<Truck>>()), Times.Once);
        }
    }
}
