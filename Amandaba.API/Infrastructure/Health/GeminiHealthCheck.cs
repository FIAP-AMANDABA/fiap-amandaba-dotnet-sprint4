using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Amandaba.API.Infrastructure.Health;

// A API externa eh checada pela disponibilidade HTTP, sem gastar tokens de IA.
// Um 401/404 ainda indica que o host respondeu; o teste nao valida a chave Gemini.
public class GeminiHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public GeminiHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    { _configuration = configuration; _httpClientFactory = httpClientFactory; }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var url = _configuration["Gemini:Url"];
        var chave = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(chave))
            return HealthCheckResult.Degraded("Integracao Gemini nao configurada.");
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
            return HealthCheckResult.Unhealthy("URL Gemini invalida.");
        try
        {
            var cliente = _httpClientFactory.CreateClient();
            cliente.Timeout = TimeSpan.FromSeconds(5);
            using var resposta = await cliente.GetAsync(new Uri(uri.GetLeftPart(UriPartial.Authority)), cancellationToken);
            return (int)resposta.StatusCode < 500
                ? HealthCheckResult.Healthy("Servico Gemini acessivel por HTTP.")
                : HealthCheckResult.Unhealthy("Servico Gemini retornou erro 5xx.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Servico Gemini indisponivel.", ex);
        }
    }
}
