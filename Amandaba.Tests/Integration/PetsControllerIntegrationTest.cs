using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using Amandaba.Application.Dtos.Pets;
using Xunit;

namespace Amandaba.Tests.Integration
{
    [Collection("Integration")]
    public class PetsControllerIntegrationTest
    {
        private readonly HttpClient _client;

        public PetsControllerIntegrationTest(
            AmandabaApiFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", AmandabaApiFactory.CriarToken(1, "TUTOR"));
        }

        [Fact]
        [Trait("Integration", "Pets")]
        public async Task ObterPorId_PetExistente_Retorna200()
        {
            // Arrange
            const decimal petId = 1;

            // Act
            var response = await _client.GetAsync(
                $"/api/pets/{petId}"
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pet = await response.Content
                .ReadFromJsonAsync<PetResponseDto>();

            Assert.NotNull(pet);
            Assert.Equal(1, pet.IdPet);
            Assert.Equal("Luna", pet.Nome);
            Assert.Equal("CACHORRO", pet.Especie);
        }

        [Fact]
        [Trait("Integration", "Pets")]
        public async Task ObterPorId_PetInexistente_Retorna404()
        {
            // Arrange
            const decimal petId = 999999;

            // Act
            var response = await _client.GetAsync(
                $"/api/pets/{petId}"
            );

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode
            );
        }

        [Fact]
        [Trait("Integration", "Pets")]
        public async Task AtualizarStatus_StatusValido_Retorna204EPersisteAlteracao()
        {
            // Arrange
            const decimal petId = 1;

            var request = new PetStatusRequestDto
            {
                Status = "INATIVO"
            };

            // Act
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/pets/{petId}/status",
                request
            );

            var getResponse = await _client.GetAsync(
                $"/api/pets/{petId}"
            );

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var pet = await getResponse.Content
                .ReadFromJsonAsync<PetResponseDto>();

            Assert.NotNull(pet);
            Assert.Equal("INATIVO", pet.Status);

            // Restaura o estado para nao afetar outros testes
            await _client.PatchAsJsonAsync(
                $"/api/pets/{petId}/status",
                new PetStatusRequestDto
                {
                    Status = "ATIVO"
                }
            );
        }
    }
}