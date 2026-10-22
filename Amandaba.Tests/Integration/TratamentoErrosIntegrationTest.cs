using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Amandaba.Application.Interfaces;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Amandaba.Tests.Integration;

[Collection("Integration")]
public class TratamentoErrosIntegrationTest
{
    private readonly AmandabaApiFactory _factory;

    public TratamentoErrosIntegrationTest(AmandabaApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ObterEspecies_FalhaInesperada_Retorna500SemDetalhesInternos()
    {
        // Arrange
        var useCase = new Mock<IEspecieUseCase>();
        useCase.Setup(servico => servico.ObterTodas())
            .Throws(new Exception("Detalhe interno de infraestrutura"));

        using var factory = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IEspecieUseCase>();
                services.AddScoped<IEspecieUseCase>(_ => useCase.Object);
            }));
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(1, "TUTOR"));

        // Act
        var resposta = await client.GetAsync("/api/especies");
        var corpo = await resposta.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, resposta.StatusCode);
        Assert.Contains("Erro interno da API.", corpo);
        Assert.DoesNotContain("Detalhe interno de infraestrutura", corpo);
    }

    [Fact]
    public async Task CadastrarPet_EspecieInvalida_Retorna400()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(1, "TUTOR"));
        var dados = new { idEspecie = 0, nome = "Luna" };

        // Act
        var resposta = await client.PostAsJsonAsync("/api/tutores/1/pets", dados);
        var corpo = await resposta.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
        Assert.Contains("Dados inválidos.", corpo);
    }
}
