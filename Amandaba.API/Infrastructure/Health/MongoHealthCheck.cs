using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amandaba.API.Infrastructure.Health;

// Health Check da aula 05. O Driver (dependencia transitiva do provider) e usado apenas para ping; persistencia usa DbContext.
public class MongoHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    public MongoHealthCheck(IConfiguration configuration) { _configuration = configuration; }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var uri = _configuration.GetConnectionString("Mongodb");
        if (string.IsNullOrWhiteSpace(uri))
            return HealthCheckResult.Degraded("MongoDB nao configurado neste ambiente.");
        try
        {
            var configuracao = MongoClientSettings.FromConnectionString(uri);
            configuracao.ServerSelectionTimeout = TimeSpan.FromSeconds(3);
            var cliente = new MongoClient(configuracao);
            var banco = cliente.GetDatabase(_configuration["MongoDb:DatabaseName"] ?? "amandaba");
            var comando = new BsonDocumentCommand<BsonDocument>(new BsonDocument("ping", 1));
            await banco.RunCommandAsync(comando, cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("MongoDB conectado e respondendo a ping.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha de conectividade com MongoDB.", ex);
        }
    }
}
