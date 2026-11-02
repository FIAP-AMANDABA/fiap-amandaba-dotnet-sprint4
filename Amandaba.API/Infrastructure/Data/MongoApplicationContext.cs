using Amandaba.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Amandaba.API.Infrastructure.Data;

// Padrão da aula: contexto MongoDB com Entity Framework Core e DbSet.
public class MongoApplicationContext : DbContext
{
    public MongoApplicationContext(DbContextOptions<MongoApplicationContext> options)
        : base(options)
    {
        // O histórico é salvo como UM documento por pet. Em MongoDB local standalone,
        // transações multi-documento não estão disponíveis; a gravação de um documento é atômica.
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    public DbSet<PesoHistoricoDocumento> HistoricosPeso { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PesoHistoricoDocumento>(entity =>
        {
            entity.ToCollection("historico_pesos");
            // As medições ficam incorporadas no mesmo documento, não em outra coleção.
            entity.OwnsMany(p => p.Historico);
        });
    }
}
