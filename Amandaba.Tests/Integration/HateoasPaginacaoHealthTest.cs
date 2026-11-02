using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Amandaba.Tests.Integration;

[Collection("Integration")]
public class HateoasPaginacaoHealthTest
{
    private readonly AmandabaApiFactory _factory;
    public HateoasPaginacaoHealthTest(AmandabaApiFactory factory) { _factory = factory; }

    [Fact]
    public async Task ObterPet_PetExistente_RetornaHateoasComMetodoHttp()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(1, "TUTOR"));

        // Act
        var resposta = await client.GetAsync("/api/pets/1");
        using var json = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var links = json.RootElement.GetProperty("links");
        Assert.Contains(links.EnumerateArray(), link =>
            link.GetProperty("rel").GetString() == "self" &&
            link.GetProperty("method").GetString() == "GET");
    }

    [Fact]
    public async Task ListarMeusPets_PaginacaoPadrao_RetornaMetadadosELinks()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(1, "TUTOR"));

        // Act
        var resposta = await client.GetAsync("/api/pets/me?pagina=1&tamanho=1");
        using var json = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal(1, json.RootElement.GetProperty("pagina").GetInt32());
        Assert.Equal(1, json.RootElement.GetProperty("tamanho").GetInt32());
        Assert.True(json.RootElement.GetProperty("total").GetInt32() >= 1);
        Assert.Equal(1, json.RootElement.GetProperty("dados").GetArrayLength());
    }

    [Fact]
    public async Task ListarEspecies_UsuarioInexistente_Retorna403()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(999, "TUTOR"));

        // Act
        var resposta = await client.GetAsync("/api/especies");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, resposta.StatusCode);
    }

    [Fact]
    public async Task ListarEspecies_UsuarioInativo_Retorna403()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(4, "TUTOR"));

        // Act
        var resposta = await client.GetAsync("/api/especies");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, resposta.StatusCode);
    }

    [Fact]
    public async Task HealthLive_SemToken_Retorna200()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var resposta = await client.GetAsync("/api/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }
}
