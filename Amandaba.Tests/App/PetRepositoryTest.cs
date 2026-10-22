using Amandaba.Domain.Entities;
using Amandaba.Infrastructure.Data;
using Amandaba.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Amandaba.Tests.App
{
    public class PetRepositoryTest
    {
        private ApplicationContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationContext(options);
        }

        [Fact]
        [Trait("PetRepository", "ObterPorId")]
        public void ObterPorId_PetExistente_RetornaPet()
        {
            // Arrange
            using var context = CriarContexto();

            var especie = new EspecieEntity
            {
                IdEspecie = 1,
                Nome = "CACHORRO"
            };

            var pet = new PetEntity
            {
                IdPet = 1,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Luna",
                Sexo = "FEMEA",
                Castrado = true,
                DataCadastro = DateTime.Now,
                Status = "ATIVO",
                Especie = especie
            };

            context.Especies.Add(especie);
            context.Pets.Add(pet);
            context.SaveChanges();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ObterPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdPet);
            Assert.Equal("Luna", resultado.Nome);
            Assert.NotNull(resultado.Especie);
            Assert.Equal("CACHORRO", resultado.Especie.Nome);
        }

        [Fact]
        [Trait("PetRepository", "ObterPorId")]
        public void ObterPorId_PetInexistente_RetornaNull()
        {
            // Arrange
            using var context = CriarContexto();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ObterPorId(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("PetRepository", "ObterPorTutor")]
        public void ObterPorTutor_PetsExistentes_RetornaPetsOrdenadosPorNome()
        {
            // Arrange
            using var context = CriarContexto();

            var especie = new EspecieEntity
            {
                IdEspecie = 1,
                Nome = "CACHORRO"
            };

            context.Especies.Add(especie);

            context.Pets.AddRange(
                new PetEntity
                {
                    IdPet = 1,
                    IdTutor = 1,
                    IdEspecie = 1,
                    Nome = "Thor",
                    DataCadastro = DateTime.Now,
                    Status = "ATIVO",
                    Especie = especie
                },
                new PetEntity
                {
                    IdPet = 2,
                    IdTutor = 1,
                    IdEspecie = 1,
                    Nome = "Luna",
                    DataCadastro = DateTime.Now,
                    Status = "ATIVO",
                    Especie = especie
                },
                new PetEntity
                {
                    IdPet = 3,
                    IdTutor = 2,
                    IdEspecie = 1,
                    Nome = "Max",
                    DataCadastro = DateTime.Now,
                    Status = "ATIVO",
                    Especie = especie
                }
            );

            context.SaveChanges();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ObterPorTutor(1).ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Luna", resultado[0].Nome);
            Assert.Equal("Thor", resultado[1].Nome);

            Assert.All(
                resultado,
                pet => Assert.Equal(1, pet.IdTutor)
            );
        }

        [Fact]
        [Trait("PetRepository", "Cadastrar")]
        public void Cadastrar_PetValido_SalvaPet()
        {
            // Arrange
            using var context = CriarContexto();

            var repository = new PetRepository(context);

            var pet = new PetEntity
            {
                IdPet = 1,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Pet Teste",
                DataCadastro = DateTime.Now,
                Status = "ATIVO"
            };

            // Act
            var resultado = repository.Cadastrar(pet);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdPet);

            var petSalvo = context.Pets
                .FirstOrDefault(p => p.IdPet == 1);

            Assert.NotNull(petSalvo);
            Assert.Equal("Pet Teste", petSalvo.Nome);
            Assert.Equal("ATIVO", petSalvo.Status);
        }

        [Fact]
        [Trait("PetRepository", "Atualizar")]
        public void Atualizar_PetExistente_SalvaAlteracoes()
        {
            // Arrange
            using var context = CriarContexto();

            var pet = new PetEntity
            {
                IdPet = 1,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Luna",
                DataCadastro = DateTime.Now,
                Status = "ATIVO"
            };

            context.Pets.Add(pet);
            context.SaveChanges();

            var repository = new PetRepository(context);

            pet.Nome = "Luna Atualizada";

            // Act
            repository.Atualizar(pet);

            // Assert
            var petAtualizado = context.Pets
                .First(p => p.IdPet == 1);

            Assert.Equal(
                "Luna Atualizada",
                petAtualizado.Nome
            );
        }

        [Fact]
        [Trait("PetRepository", "ExisteTutor")]
        public void ExisteTutor_TutorExistente_RetornaTrue()
        {
            // Arrange
            using var context = CriarContexto();

            var usuario = new UsuarioEntity
            {
                IdUsuario = 1,
                Nome = "Usuario Teste",
                Cpf = "12345678901",
                Email = "teste@teste.com",
                SenhaHash = "hash",
                DataCadastro = DateTime.Now,
                Status = "ATIVO"
            };

            var tutor = new TutorEntity
            {
                IdTutor = 1,
                IdUsuario = 1,
                Usuario = usuario
            };

            context.Usuarios.Add(usuario);
            context.Tutores.Add(tutor);
            context.SaveChanges();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ExisteTutor(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("PetRepository", "ExisteTutor")]
        public void ExisteTutor_TutorInexistente_RetornaFalse()
        {
            // Arrange
            using var context = CriarContexto();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ExisteTutor(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        [Trait("PetRepository", "ExisteEspecie")]
        public void ExisteEspecie_EspecieExistente_RetornaTrue()
        {
            // Arrange
            using var context = CriarContexto();

            var especie = new EspecieEntity
            {
                IdEspecie = 1,
                Nome = "CACHORRO"
            };

            context.Especies.Add(especie);
            context.SaveChanges();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ExisteEspecie(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("PetRepository", "ExisteEspecie")]
        public void ExisteEspecie_EspecieInexistente_RetornaFalse()
        {
            // Arrange
            using var context = CriarContexto();

            var repository = new PetRepository(context);

            // Act
            var resultado = repository.ExisteEspecie(999);

            // Assert
            Assert.False(resultado);
        }
    }
}