using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Amandaba.Tests.Integration;

[Collection("Integration")]
public class JwtECompartilhamentoIntegrationTest
{
    private readonly AmandabaApiFactory _factory;

    public JwtECompartilhamentoIntegrationTest(AmandabaApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ObterPet_SemToken_Retorna401()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/pets/1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ObterPet_TutorDiferente_Retorna403()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(3, "TUTOR"));
        var response = await client.GetAsync("/api/pets/1");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ObterPet_VeterinarioCompartilhado_Retorna200()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var response = await client.GetAsync("/api/pets/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AtualizarPet_VeterinarioCompartilhado_Retorna403()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var response = await client.PatchAsJsonAsync("/api/pets/1/status", new { status = "INATIVO" });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CadastrarPeso_VeterinarioComAcesso_Retorna201()
    {
        var repositorio = new Mock<IPesoRepository>();
        repositorio.Setup(r => r.ExistePet(1m)).Returns(true);
        repositorio.Setup(r => r.Cadastrar(It.IsAny<PetPesoEntity>()))
            .Returns((PetPesoEntity peso) =>
            {
                peso.IdHistoricoPeso = 10m;
                return peso;
            });

        using var factory = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IPesoRepository>();
                services.AddScoped<IPesoRepository>(_ => repositorio.Object);
            }));

        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(2, "VETERINARIO"));

        var dto = new { peso = 28.5m, dataMedicao = new DateTime(2026, 10, 1) };
        var response = await client.PostAsJsonAsync("/api/pets/1/pesos", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        repositorio.Verify(r => r.Cadastrar(It.Is<PetPesoEntity>(p =>
            p.IdPet == 1m && p.Peso == 28.5m)), Times.Once);
    }

    [Fact]
    public async Task AlterarStatus_VeterinarioComAcesso_Retorna403()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", AmandabaApiFactory.CriarToken(2, "VETERINARIO"));
        var response = await client.PatchAsJsonAsync("/api/pets/1/status", new { status = "INATIVO" });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
