using System.Globalization;
using Application.Logic;
using Domain.Contracts;
using Domain.Enumerables;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Presentation.EndPoints;

namespace tests
{
    public class TrucksEndPointsTests
    {
        private readonly Mock<ITruckBusinessLogic> mockLogic;

        public TrucksEndPointsTests()
        {
            mockLogic = new Mock<ITruckBusinessLogic>();
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenTruckIsAdded()
        {
            var request = new CreateTruckRequest
            {
                Model = TruckModel.FH,
                ManufacturingYear = 2022,
                ChassisCode = "XYZ123456789",
                Color = "Red",
                PlantIsoCode = PlantLocation.US,
            };

            mockLogic
                .Setup(logic => logic.AddTruck(request))
                .ReturnsAsync(
                    new TrucksResponse(
                        Id: Guid.NewGuid(),
                        Model: TruckModel.FH.GetDescription(),
                        ManufacturingYear: 2022,
                        ChassisCode: "ABC123456789",
                        Color: "Blue",
                        PlantName: PlantLocation.BR.GetDescription(),
                        CreatedAt: DateTime.Now.ToString("G", CultureInfo.CurrentCulture)
                    )
                );

            var result = await TrucksEndPoints.Add(mockLogic.Object, request);

            Assert.IsType<Ok<TrucksResponse>>(result);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_OnException()
        {
            var request = new CreateTruckRequest
            {
                Model = TruckModel.FH,
                ManufacturingYear = 2022,
                ChassisCode = "XYZ123456789",
                Color = "Red",
                PlantIsoCode = PlantLocation.US,
            };

            mockLogic
                .Setup(logic => logic.AddTruck(request))
                .ThrowsAsync(new Exception("Erro ao adicionar caminhão"));

            var result = await TrucksEndPoints.Add(mockLogic.Object, request);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenTruckIsUpdated()
        {
            var request = new UpdateTruckRequest(
                Id: Guid.NewGuid(),
                Model: TruckModel.FM,
                ManufacturingYear: 2021,
                ChassisCode: "DEF987654321",
                Color: "Blue",
                PlantIsoCode: PlantLocation.BR
            );

            mockLogic
                .Setup(logic => logic.UpdateTruck(request))
                .ReturnsAsync(
                    new TrucksResponse(
                        Id: Guid.NewGuid(),
                        Model: TruckModel.FH.GetDescription(),
                        ManufacturingYear: 2022,
                        ChassisCode: "ABC123456789",
                        Color: "Blue",
                        PlantName: PlantLocation.BR.GetDescription(),
                        CreatedAt: DateTime.Now.ToString("G", CultureInfo.CurrentCulture)
                    )
                );

            var result = await TrucksEndPoints.Update(mockLogic.Object, request);

            Assert.IsType<Ok<TrucksResponse>>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_OnException()
        {
            var request = new UpdateTruckRequest(
                Id: Guid.NewGuid(),
                Model: TruckModel.FM,
                ManufacturingYear: 2021,
                ChassisCode: "DEF987654321",
                Color: "Blue",
                PlantIsoCode: PlantLocation.BR
            );

            mockLogic
                .Setup(logic => logic.UpdateTruck(request))
                .ThrowsAsync(new Exception("Erro ao atualizar caminhão"));

            var result = await TrucksEndPoints.Update(mockLogic.Object, request);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithTrucks()
        {
            var trucks = new List<TrucksResponse>
            {
                new TrucksResponse(
                    Id: Guid.NewGuid(),
                    Model: TruckModel.FH.GetDescription(),
                    ManufacturingYear: 2022,
                    ChassisCode: "ABC123456789",
                    Color: "Blue",
                    PlantName: PlantLocation.BR.GetDescription(),
                    CreatedAt: DateTime.Now.ToString("G", CultureInfo.CurrentCulture)
                ),
                new TrucksResponse(
                    Id: Guid.NewGuid(),
                    Model: TruckModel.VM.GetDescription(),
                    ManufacturingYear: 2023,
                    ChassisCode: "XYZ987654321",
                    Color: "Blue",
                    PlantName: PlantLocation.SE.GetDescription(),
                    CreatedAt: DateTime.Now.ToString("G", CultureInfo.CurrentCulture)
                ),
            };

            mockLogic.Setup(logic => logic.GetAll()).ReturnsAsync(trucks);

            var result = await TrucksEndPoints.GetAll(mockLogic.Object);

            Assert.IsType<Ok<List<TrucksResponse>>>(result);
            var okResult = result as Ok<List<TrucksResponse>>;
            Assert.Equal(trucks, okResult?.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnBadRequest_OnException()
        {
            mockLogic
                .Setup(logic => logic.GetAll())
                .ThrowsAsync(new Exception("Erro ao buscar caminhões"));

            var result = await TrucksEndPoints.GetAll(mockLogic.Object);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WithTruck()
        {
            var truckId = Guid.NewGuid();
            var truck = new TrucksResponse(
                Id: truckId,
                Model: TruckModel.FH.GetDescription(),
                ManufacturingYear: 2022,
                ChassisCode: "XYZ123456789",
                Color: "Red",
                PlantName: PlantLocation.US.GetDescription(),
                CreatedAt: DateTime.Now.ToString("G", CultureInfo.CurrentCulture)
            );

            mockLogic.Setup(logic => logic.GetById(truckId)).ReturnsAsync(truck);

            var result = await TrucksEndPoints.GetById(mockLogic.Object, truckId);

            Assert.IsType<Ok<TrucksResponse>>(result);
            var okResult = result as Ok<TrucksResponse>;
            Assert.Equal(truck, okResult?.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_OnException()
        {
            var truckId = Guid.NewGuid();

            mockLogic
                .Setup(logic => logic.GetById(truckId))
                .ThrowsAsync(new Exception("Erro ao buscar caminhão"));

            var result = await TrucksEndPoints.GetById(mockLogic.Object, truckId);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task DeleteRange_ShouldReturnOk_WhenTrucksAreDeleted()
        {
            var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            mockLogic.Setup(logic => logic.DeleteTrucks(ids)).Returns(Task.CompletedTask);

            var result = await TrucksEndPoints.DeleteRange(mockLogic.Object, ids);

            Assert.IsType<Ok<string>>(result);
        }

        [Fact]
        public async Task DeleteRange_ShouldReturnBadRequest_OnException()
        {
            var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            mockLogic
                .Setup(logic => logic.DeleteTrucks(ids))
                .ThrowsAsync(new Exception("Erro ao deletar caminhões"));

            var result = await TrucksEndPoints.DeleteRange(mockLogic.Object, ids);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task Generate100_ShouldReturnOk_WhenTrucksGenerated()
        {
            mockLogic
                .Setup(logic => logic.GenerateAndAdd100Trucks())
                .ReturnsAsync(new List<TrucksResponse>());

            var result = await TrucksEndPoints.Generate100(mockLogic.Object);

            Assert.IsType<Ok<List<TrucksResponse>>>(result);
        }

        [Fact]
        public async Task Generate100_ShouldReturnBadRequest_OnException()
        {
            mockLogic
                .Setup(logic => logic.GenerateAndAdd100Trucks())
                .ThrowsAsync(new Exception("Erro ao gerar caminhões"));

            var result = await TrucksEndPoints.Generate100(mockLogic.Object);

            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public async Task FillDashboard_ShouldReturnOk_WithDashboardInfo()
        {
            var dashboardInfo = new DashboardInfoResponse(1, [], [], []);

            mockLogic.Setup(logic => logic.GetDashboardInfo()).ReturnsAsync(dashboardInfo);

            var result = await TrucksEndPoints.FillDashboard(mockLogic.Object);

            Assert.IsType<Ok<DashboardInfoResponse>>(result);
            var okResult = result as Ok<DashboardInfoResponse>;
            Assert.Equal(dashboardInfo, okResult?.Value);
        }

        [Fact]
        public async Task FillDashboard_ShouldReturnBadRequest_OnException()
        {
            mockLogic
                .Setup(logic => logic.GetDashboardInfo())
                .ThrowsAsync(new Exception("Erro ao carregar o dashboard"));

            var result = await TrucksEndPoints.FillDashboard(mockLogic.Object);

            Assert.IsType<BadRequest<string>>(result);
        }
    }
}
