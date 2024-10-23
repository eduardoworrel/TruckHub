using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace tests;

public class DependencyInjectionTest
{
    private readonly IConfiguration _configuration;

    public DependencyInjectionTest()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:APP_CONNECTIONSTRING"] = "your_connection_string",
                }
            )
            .Build();
    }

    [Fact]
    public void AddDatabase_ShouldAddDbContext()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDatabase(_configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        Assert.NotNull(dbContext);
    }

    [Fact]
    public void AddRepositories_ShouldAddRepositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDatabase(_configuration);
        services.AddRepositories();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetRequiredService<ITruckRepository>());
    }
}
