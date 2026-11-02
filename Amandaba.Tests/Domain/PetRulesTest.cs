using Amandaba.Domain.Rules;

namespace Amandaba.Tests.Domain;

public class PetRulesTest
{
    [Theory]
    [InlineData("ativo", "ATIVO")]
    [InlineData(" inativo ", "INATIVO")]
    public void ValidarStatus_ValorPermitido_Normaliza(string entrada, string esperado)
    {
        // Arrange
        var status = entrada;
        // Act
        var resultado = PetRules.ValidarStatus(status);
        // Assert
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("EM_TRATAMENTO")]
    [InlineData("")]
    [InlineData(null)]
    public void ValidarStatus_ValorInvalido_LancaArgumentException(string? entrada)
    {
        // Arrange
        var status = entrada;
        // Act
        var erro = Record.Exception(() => PetRules.ValidarStatus(status));
        // Assert
        Assert.IsType<ArgumentException>(erro);
    }

    [Theory]
    [InlineData("macho", "MACHO")]
    [InlineData(" FEMEA ", "FEMEA")]
    public void ValidarSexo_ValorPermitido_Normaliza(string entrada, string esperado)
    {
        // Arrange
        var sexo = entrada;
        // Act
        var resultado = PetRules.ValidarSexo(sexo);
        // Assert
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void ValidarSexo_ValorInvalido_LancaArgumentException()
    {
        // Arrange
        const string sexo = "OUTRO";
        // Act
        var erro = Record.Exception(() => PetRules.ValidarSexo(sexo));
        // Assert
        Assert.IsType<ArgumentException>(erro);
    }

    [Fact]
    public void ValidarNascimento_DataFutura_LancaArgumentException()
    {
        // Arrange
        var nascimento = DateTime.Today.AddDays(1);
        // Act
        var erro = Record.Exception(() => PetRules.ValidarNascimento(nascimento));
        // Assert
        Assert.IsType<ArgumentException>(erro);
    }

    [Fact]
    public void ValidarNascimento_DataPassada_NaoLancaExcecao()
    {
        // Arrange
        var nascimento = DateTime.Today.AddYears(-2);
        // Act
        var erro = Record.Exception(() => PetRules.ValidarNascimento(nascimento));
        // Assert
        Assert.Null(erro);
    }

    [Fact]
    public void ValidarNome_EspacosEmBranco_LancaArgumentException()
    {
        // Arrange
        const string nome = "  ";
        // Act
        var erro = Record.Exception(() => PetRules.ValidarNome(nome));
        // Assert
        Assert.IsType<ArgumentException>(erro);
    }

    [Fact]
    public void ValidarNome_NomeValido_RemoveEspacos()
    {
        // Arrange
        const string nome = " Luna ";
        // Act
        var resultado = PetRules.ValidarNome(nome);
        // Assert
        Assert.Equal("Luna", resultado);
    }
}
