using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Amandaba.Tests.Integration;
[Collection("Integration")]
public class JwtECompartilhamentoIntegrationTest
{
    private readonly AmandabaApiFactory _factory;
    public JwtECompartilhamentoIntegrationTest(AmandabaApiFactory factory) { _factory = factory; }

    [Fact]
    public async Task ObterPet_SemToken_Retorna401()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("/api/pets/1");
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ObterPet_TutorDiferente_Retorna403()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            AmandabaApiFactory.CriarToken(3, "TUTOR"));
        var response = await client.GetAsync("/api/pets/1");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ObterPet_VeterinarioCompartilhado_Retorna200()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var response = await client.GetAsync("/api/pets/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AtualizarPet_VeterinarioCompartilhado_Retorna403()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var response = await client.PatchAsJsonAsync("/api/pets/1/status", new { status = "INATIVO" });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CadastrarPeso_VeterinarioComAcesso_Retorna201()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var dto = new { peso = 28.5m, dataMedicao = new DateTime(2026, 10, 1) };
        // Act
        var response = await client.PostAsJsonAsync("/api/pets/1/pesos", dto);
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task AlterarStatus_VeterinarioComAcesso_Retorna403()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        // Act
        var response = await client.PatchAsJsonAsync("/api/pets/1/status", new { status = "INATIVO" });
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
