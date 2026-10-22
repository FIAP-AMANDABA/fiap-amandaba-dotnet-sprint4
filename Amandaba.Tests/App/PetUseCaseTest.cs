using Amandaba.API.Domain.Interfaces;
using Amandaba.Application.Dtos.Pets;
using Amandaba.Application.UseCases;
using Amandaba.Domain.Entities;
using Moq;
using Xunit;

namespace Amandaba.Tests.App
{
    public class PetUseCaseTest
    {
        private readonly Mock<IPetRepository> _petRepositoryMock;
        private readonly PetUseCase _petUseCase;

        public PetUseCaseTest()
        {
            _petRepositoryMock = new Mock<IPetRepository>();
            _petUseCase = new PetUseCase(_petRepositoryMock.Object);
        }

        [Fact]
        [Trait("PetUseCase", "ObterPorTutor")]
        public void ObterPorTutor_TutorInexistente_LancaKeyNotFoundException()
        {
            // Arrange
            decimal idTutor = 999;

            _petRepositoryMock
                .Setup(repository => repository.ExisteTutor(idTutor))
                .Returns(false);

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(
                () => _petUseCase.ObterPorTutor(idTutor)
            );

            // Assert
            Assert.Equal("Tutor não encontrado.", exception.Message);

            _petRepositoryMock.Verify(
                repository => repository.ExisteTutor(idTutor),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.ObterPorTutor(It.IsAny<decimal>()),
                Times.Never
            );
        }

        [Fact]
        [Trait("PetUseCase", "ObterPorTutor")]
        public void ObterPorTutor_TutorExistente_RetornaPets()
        {
            // Arrange
            decimal idTutor = 1;

            var especie = new EspecieEntity
            {
                IdEspecie = 1,
                Nome = "CACHORRO"
            };

            var pets = new List<PetEntity>
            {
                new PetEntity
                {
                    IdPet = 1,
                    IdTutor = idTutor,
                    IdEspecie = 1,
                    Nome = "Luna",
                    Sexo = "FEMEA",
                    Status = "ATIVO",
                    Especie = especie
                },
                new PetEntity
                {
                    IdPet = 2,
                    IdTutor = idTutor,
                    IdEspecie = 1,
                    Nome = "Max",
                    Sexo = "MACHO",
                    Status = "ATIVO",
                    Especie = especie
                }
            };

            _petRepositoryMock
                .Setup(repository => repository.ExisteTutor(idTutor))
                .Returns(true);

            _petRepositoryMock
                .Setup(repository => repository.ObterPorTutor(idTutor))
                .Returns(pets);

            // Act
            var resultado = _petUseCase.ObterPorTutor(idTutor).ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Luna", resultado[0].Nome);
            Assert.Equal("Max", resultado[1].Nome);
            Assert.Equal("CACHORRO", resultado[0].Especie);

            _petRepositoryMock.Verify(
                repository => repository.ExisteTutor(idTutor),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.ObterPorTutor(idTutor),
                Times.Once
            );
        }

        [Fact]
        [Trait("PetUseCase", "ObterPorId")]
        public void ObterPorId_PetInexistente_RetornaNull()
        {
            // Arrange
            decimal idPet = 999;

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns((PetEntity?)null);

            // Act
            var resultado = _petUseCase.ObterPorId(idPet);

            // Assert
            Assert.Null(resultado);

            _petRepositoryMock.Verify(
                repository => repository.ObterPorId(idPet),
                Times.Once
            );
        }

        [Fact]
        [Trait("PetUseCase", "ObterPorId")]
        public void ObterPorId_PetExistente_RetornaPet()
        {
            // Arrange
            decimal idPet = 1;

            var pet = new PetEntity
            {
                IdPet = idPet,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Luna",
                Raca = "Golden Retriever",
                Sexo = "FEMEA",
                Castrado = true,
                Status = "ATIVO",
                Especie = new EspecieEntity
                {
                    IdEspecie = 1,
                    Nome = "CACHORRO"
                }
            };

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns(pet);

            // Act
            var resultado = _petUseCase.ObterPorId(idPet);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idPet, resultado.IdPet);
            Assert.Equal("Luna", resultado.Nome);
            Assert.Equal("CACHORRO", resultado.Especie);
            Assert.Equal("ATIVO", resultado.Status);

            _petRepositoryMock.Verify(
                repository => repository.ObterPorId(idPet),
                Times.Once
            );
        }

        [Fact]
        [Trait("PetUseCase", "Cadastrar")]
        public void Cadastrar_TutorInexistente_LancaKeyNotFoundException()
        {
            // Arrange
            decimal idTutor = 999;

            var dto = new PetRequestDto();

            _petRepositoryMock
                .Setup(repository => repository.ExisteTutor(idTutor))
                .Returns(false);

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(
                () => _petUseCase.Cadastrar(idTutor, dto)
            );

            // Assert
            Assert.Equal("Tutor não encontrado.", exception.Message);

            _petRepositoryMock.Verify(
                repository => repository.ExisteTutor(idTutor),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.Cadastrar(It.IsAny<PetEntity>()),
                Times.Never
            );
        }

        [Fact]
        [Trait("PetUseCase", "Cadastrar")]
        public void Cadastrar_DadosValidos_RetornaPetCadastrado()
        {
            // Arrange
            decimal idTutor = 1;

            var dto = new PetRequestDto
            {
                IdEspecie = 1,
                Nome = "Pet Teste",
                Raca = "SRD",
                Sexo = "MACHO",
                DataNascimento = new DateTime(2022, 5, 15),
                Cor = "Caramelo",
                Castrado = false,
                Microchip = "TESTE-001"
            };

            var petCadastrado = new PetEntity
            {
                IdPet = 18,
                IdTutor = idTutor,
                IdEspecie = 1,
                Nome = "Pet Teste",
                Raca = "SRD",
                Sexo = "MACHO",
                DataNascimento = new DateTime(2022, 5, 15),
                Cor = "Caramelo",
                Castrado = false,
                Microchip = "TESTE-001",
                Status = "ATIVO",
                Especie = new EspecieEntity
                {
                    IdEspecie = 1,
                    Nome = "CACHORRO"
                }
            };

            _petRepositoryMock
                .Setup(repository => repository.ExisteTutor(idTutor))
                .Returns(true);

            _petRepositoryMock
                .Setup(repository => repository.ExisteEspecie(dto.IdEspecie))
                .Returns(true);

            _petRepositoryMock
                .Setup(repository => repository.Cadastrar(It.IsAny<PetEntity>()))
                .Returns((PetEntity pet) =>
                {
                    pet.IdPet = 18;
                    return pet;
                });

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(18))
                .Returns(petCadastrado);

            // Act
            var resultado = _petUseCase.Cadastrar(idTutor, dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(18, resultado.IdPet);
            Assert.Equal("Pet Teste", resultado.Nome);
            Assert.Equal("CACHORRO", resultado.Especie);
            Assert.Equal("ATIVO", resultado.Status);

            _petRepositoryMock.Verify(
                repository => repository.ExisteTutor(idTutor),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.ExisteEspecie(1),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.Cadastrar(It.IsAny<PetEntity>()),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.ObterPorId(18),
                Times.Once
            );
        }

        [Fact]
        [Trait("PetUseCase", "Atualizar")]
        public void Atualizar_PetInexistente_RetornaFalse()
        {
            // Arrange
            decimal idPet = 999;

            var dto = new PetRequestDto();

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns((PetEntity?)null);

            // Act
            var resultado = _petUseCase.Atualizar(idPet, dto);

            // Assert
            Assert.False(resultado);

            _petRepositoryMock.Verify(
                repository => repository.ObterPorId(idPet),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.Atualizar(It.IsAny<PetEntity>()),
                Times.Never
            );
        }

        [Fact]
        [Trait("PetUseCase", "Atualizar")]
        public void Atualizar_DadosValidos_AtualizaPetERetornaTrue()
        {
            // Arrange
            decimal idPet = 1;

            var pet = new PetEntity
            {
                IdPet = idPet,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Luna",
                Sexo = "FEMEA",
                Status = "ATIVO"
            };

            var dto = new PetRequestDto
            {
                IdEspecie = 2,
                Nome = "Luna Atualizada",
                Raca = "SRD",
                Sexo = "femea",
                Cor = "Branco",
                Castrado = true,
                Microchip = "ABC-123"
            };

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns(pet);

            _petRepositoryMock
                .Setup(repository => repository.ExisteEspecie(dto.IdEspecie))
                .Returns(true);

            // Act
            var resultado = _petUseCase.Atualizar(idPet, dto);

            // Assert
            Assert.True(resultado);
            Assert.Equal(2, pet.IdEspecie);
            Assert.Equal("Luna Atualizada", pet.Nome);
            Assert.Equal("FEMEA", pet.Sexo);
            Assert.Equal("Branco", pet.Cor);
            Assert.True(pet.Castrado);
            Assert.Equal("ABC-123", pet.Microchip);

            _petRepositoryMock.Verify(
                repository => repository.Atualizar(pet),
                Times.Once
            );
        }

        [Fact]
        [Trait("PetUseCase", "AtualizarStatus")]
        public void AtualizarStatus_StatusInvalido_LancaArgumentException()
        {
            // Arrange
            decimal idPet = 1;

            var pet = new PetEntity();

            var dto = new PetStatusRequestDto
            {
                Status = "BLOQUEADO"
            };

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns(pet);

            // Act
            var exception = Assert.Throws<ArgumentException>(
                () => _petUseCase.AtualizarStatus(idPet, dto)
            );

            // Assert
            Assert.Equal(
                "Status inválido. Utilize ATIVO ou INATIVO.",
                exception.Message
            );

            _petRepositoryMock.Verify(
                repository => repository.ObterPorId(idPet),
                Times.Once
            );

            _petRepositoryMock.Verify(
                repository => repository.Atualizar(It.IsAny<PetEntity>()),
                Times.Never
            );
        }

        [Fact]
        [Trait("PetUseCase", "AtualizarStatus")]
        public void AtualizarStatus_StatusValido_AtualizaStatusERetornaTrue()
        {
            // Arrange
            decimal idPet = 1;

            var pet = new PetEntity
            {
                IdPet = idPet,
                Status = "ATIVO"
            };

            var dto = new PetStatusRequestDto
            {
                Status = "inativo"
            };

            _petRepositoryMock
                .Setup(repository => repository.ObterPorId(idPet))
                .Returns(pet);

            // Act
            var resultado = _petUseCase.AtualizarStatus(idPet, dto);

            // Assert
            Assert.True(resultado);
            Assert.Equal("INATIVO", pet.Status);

            _petRepositoryMock.Verify(
                repository => repository.Atualizar(pet),
                Times.Once
            );
        }
    }
}