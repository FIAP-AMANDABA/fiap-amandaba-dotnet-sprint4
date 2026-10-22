using Amandaba.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.API.Presentation.Controllers;

[ApiController]
[Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class IntegracaoOracleController : ControllerBase
{
    private readonly ApplicationContext _db;
    public IntegracaoOracleController(ApplicationContext db) { _db = db; }

    // O SQL final da disciplina recebe o ID_TUTOR, nao ID_PET.
    // Esta rota esta protegida pelo filtro que confere o tutor autenticado.
    [HttpPost("api/tutores/{idTutor}/integracao-oracle/executar-pr-pet-json")]
    public async Task<IActionResult> ExecutarProcedure(decimal idTutor)
    {
        var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "BEGIN PKG_PET_SAUDE.PR_PET_JSON(:idTutor); END;";
            var parametro = cmd.CreateParameter();
            parametro.ParameterName = "idTutor";
            parametro.Value = idTutor;
            cmd.Parameters.Add(parametro);
            await cmd.ExecuteNonQueryAsync();
            return Ok(new { mensagem = "PR_PET_JSON executada. Saida textual disponivel via DBMS_OUTPUT no Oracle.", idTutor });
        }
        finally { await conn.CloseAsync(); }
    }

    // O SQL final da disciplina recebe uma DATA de nascimento, nao ID_PET.
    [HttpGet("api/pets/{petId}/integracao-oracle/idade")]
    public async Task<IActionResult> CalcularIdade(decimal petId)
    {
        var nascimento = await _db.Pets.AsNoTracking().Where(p => p.IdPet == petId)
            .Select(p => p.DataNascimento).FirstOrDefaultAsync();
        if (nascimento is null) return BadRequest(new { mensagem = "Pet sem data de nascimento." });

        var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PKG_PET_SAUDE.FN_CALCULAR_IDADE_PET(:nascimento) FROM DUAL";
            var parametro = cmd.CreateParameter();
            parametro.ParameterName = "nascimento";
            parametro.Value = nascimento.Value;
            cmd.Parameters.Add(parametro);
            var valor = await cmd.ExecuteScalarAsync();
            return Ok(new { petId, idadeAnos = valor is null || valor == DBNull.Value ? (int?)null : Convert.ToInt32(valor) });
        }
        finally { await conn.CloseAsync(); }
    }
}
