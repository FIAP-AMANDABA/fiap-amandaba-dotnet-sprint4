using Amandaba.API.Application.UseCases;
using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Moq;
using Xunit;

namespace Amandaba.Tests.App;
public class CompartilhamentoUseCaseTest
{
    [Fact]
    public void Compartilhar_VinculoRevogado_ReativaMesmoRegistro()
    {
        // Arrange
        var vinculo = new PetVeterinarioEntity { IdPetVeterinario = 9, IdPet = 1,
            IdVeterinario = 7, Status = "REVOGADO" };
        var repo = new Mock<ICompartilhamentoRepository>();
        repo.Setup(r => r.BuscarVeterinario(7)).Returns(new VeterinarioEntity
            { IdVeterinario = 7, Status = "ATIVO" });
        repo.Setup(r => r.BuscarVinculo(1, 7)).Returns(vinculo);
        var servico = new CompartilhamentoUseCase(repo.Object);
        // Act
        var atualizado = servico.Compartilhar(1, 7);
        // Assert
        Assert.Equal(9, atualizado.IdPetVeterinario);
        Assert.Equal("ATIVO", atualizado.Status);
        repo.Verify(r => r.Salvar(vinculo), Times.Once);
    }

    [Fact]
    public void Compartilhar_VeterinarioInativo_LancaArgumentException()
    {
        // Arrange
        var repo = new Mock<ICompartilhamentoRepository>();
        repo.Setup(r => r.BuscarVeterinario(7)).Returns(new VeterinarioEntity { Status = "INATIVO" });
        var servico = new CompartilhamentoUseCase(repo.Object);
        // Act / Assert
        Assert.Throws<ArgumentException>(() => servico.Compartilhar(1, 7));
        repo.Verify(r => r.Salvar(It.IsAny<PetVeterinarioEntity>()), Times.Never);
    }
}
