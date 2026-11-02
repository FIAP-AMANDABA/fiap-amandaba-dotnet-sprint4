using Amandaba.API.Infrastructure.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace Amandaba.Tests.App;

public class HealthChecksTest
{
    [Fact]
    public async Task MongoHealthCheck_SemConexao_RetornaDegraded()
    {
        // Arrange
        var config = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?> { ["ConnectionStrings:Mongodb"] = "" }).Build();
        var health = new MongoHealthCheck(config);

        // Act
        var resultado = await health.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Degraded, resultado.Status);
    }

    [Fact]
    public async Task GeminiHealthCheck_SemChave_RetornaDegraded()
    {
        // Arrange
        var config = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?> { ["Gemini:ApiKey"] = "" }).Build();
        var health = new GeminiHealthCheck(config, new Mock<IHttpClientFactory>().Object);

        // Act
        var resultado = await health.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Degraded, resultado.Status);
    }
}
