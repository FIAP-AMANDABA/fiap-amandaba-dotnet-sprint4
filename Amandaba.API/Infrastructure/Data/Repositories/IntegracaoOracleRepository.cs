using System.Data;
using Amandaba.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories;

// O SQL permanece na Infrastructure, e nao no Controller (aula 07).
public class IntegracaoOracleRepository : IIntegracaoOracleRepository
{
    private readonly ApplicationContext _db;
    public IntegracaoOracleRepository(ApplicationContext db) { _db = db; }

    public async Task ExecutarPetJsonAsync(decimal idTutor)
    {
        var conexao = _db.Database.GetDbConnection();
        await conexao.OpenAsync();
        try
        {
            using var cmd = conexao.CreateCommand();
            cmd.CommandText = "BEGIN PKG_PET_SAUDE.PR_PET_JSON(:idTutor); END;";
            var parametro = cmd.CreateParameter();
            parametro.ParameterName = "idTutor";
            parametro.Value = idTutor;
            cmd.Parameters.Add(parametro);
            await cmd.ExecuteNonQueryAsync();
        }
        finally { await conexao.CloseAsync(); }
    }

    public async Task<int?> CalcularIdadePetAsync(decimal petId)
    {
        var nascimento = await _db.Pets.AsNoTracking()
            .Where(p => p.IdPet == petId).Select(p => p.DataNascimento).FirstOrDefaultAsync();
        if (nascimento is null)
            throw new ArgumentException("Pet sem data de nascimento.");

        var conexao = _db.Database.GetDbConnection();
        await conexao.OpenAsync();
        try
        {
            using var cmd = conexao.CreateCommand();
            cmd.CommandText = "SELECT PKG_PET_SAUDE.FN_CALCULAR_IDADE_PET(:nascimento) FROM DUAL";
            var parametro = cmd.CreateParameter();
            parametro.ParameterName = "nascimento";
            parametro.Value = nascimento.Value;
            cmd.Parameters.Add(parametro);
            var valor = await cmd.ExecuteScalarAsync();
            return valor is null || valor == DBNull.Value ? null : Convert.ToInt32(valor);
        }
        finally { await conexao.CloseAsync(); }
    }
}
