using Domain.Contracts;
using Domain.Entities;
using Domain.Enumerables;

namespace tests;

public class TruckEntityTest
{
    [Fact]
    public void Create_ShouldGenerateTypeOfTruck()
    {
        var request = new CreateTruckRequest
        {
            Model = TruckModel.FH,
            ManufacturingYear = 2022,
            ChassisCode = "XYZ123456789",
            Color = "Red",
            PlantIsoCode = PlantLocation.US,
        };

        var truck = Truck.Create(request);

        Assert.IsType<Truck>(truck);
        Assert.Equal(request.Model, truck.Model);
        Assert.Equal(request.ManufacturingYear, truck.ManufacturingYear);
        Assert.Equal(request.ChassisCode, truck.ChassisCode);
        Assert.Equal(request.Color, truck.Color);
        Assert.Equal(request.PlantIsoCode, truck.Plant);
    }

    [Fact]
    public void Update_ShouldUpdateTruckWithNewValues()
    {
        // Arrange
        var truck = Truck.Create(
            new CreateTruckRequest
            {
                Model = TruckModel.VM,
                ManufacturingYear = 2020,
                ChassisCode = "ABC123456789",
                Color = "Blue",
                PlantIsoCode = PlantLocation.BR,
            }
        );

        var updateRequest = new UpdateTruckRequest(
            truck.Id,
            TruckModel.FH,
            2021,
            "DEF987654321",
            "Green",
            PlantLocation.FR
        );

        // Act
        truck.Update(updateRequest);

        // Assert
        Assert.Equal(updateRequest.Model, truck.Model);
        Assert.Equal(updateRequest.ManufacturingYear, truck.ManufacturingYear);
        Assert.Equal(updateRequest.ChassisCode, truck.ChassisCode);
        Assert.Equal(updateRequest.Color, truck.Color);
        Assert.Equal(updateRequest.PlantIsoCode, truck.Plant);
    }

    [Fact]
    public void ToResponse_ShouldReturnCorrectTruckResponse()
    {
        // Arrange
        var request = new CreateTruckRequest
        {
            Model = TruckModel.FM,
            ManufacturingYear = 2023,
            ChassisCode = "GHJ123456789",
            Color = "Black",
            PlantIsoCode = PlantLocation.SE,
        };

        var truck = Truck.Create(request);

        // Act
        var response = truck.ToResponse();

        // Assert
        Assert.Equal(truck.Id, response.Id);
        Assert.Equal(truck.Model.GetDescription(), response.Model);
        Assert.Equal(truck.ManufacturingYear, response.ManufacturingYear);
        Assert.Equal(truck.ChassisCode, response.ChassisCode);
        Assert.Equal(truck.Color, response.Color);
        Assert.Equal(truck.Plant.GetDescription(), response.PlantName);
        Assert.NotNull(response.CreatedAt);
    }
}
