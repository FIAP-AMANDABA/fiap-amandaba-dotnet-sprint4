using Amandaba.API.Domain.Entities;
using Amandaba.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Tests.App;

public class MongoModelTest
{
    [Fact]
    public void MongoApplicationContext_HistoricoDePesos_ModeloComDocumentoIncorporado()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MongoApplicationContext>()
            .UseMongoDB("mongodb://localhost:27017", "amandaba")
            .Options;
        using var context = new MongoApplicationContext(options);

        // Act
        var tipoDocumento = context.Model.FindEntityType(typeof(PesoHistoricoDocumento));
        var colecao = tipoDocumento?.FindNavigation(nameof(PesoHistoricoDocumento.Historico));

        // Assert
        Assert.NotNull(tipoDocumento);
        Assert.NotNull(tipoDocumento!.FindPrimaryKey());
        Assert.NotNull(colecao);
        Assert.True(colecao!.ForeignKey.IsOwnership);
    }
}
