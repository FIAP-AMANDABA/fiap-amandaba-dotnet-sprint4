using Amandaba.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amandaba.API.Presentation.Controllers;

[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class IntegracaoOracleController : ControllerBase
{
    private readonly IIntegracaoOracleUseCase _integracao;
    public IntegracaoOracleController(IIntegracaoOracleUseCase integracao)
    { _integracao = integracao; }

    // A procedure recebe idTutor e produz DBMS_OUTPUT no lado Oracle.
    [HttpPost("api/tutores/{idTutor}/integracao-oracle/executar-pr-pet-json")]
    public async Task<IActionResult> ExecutarProcedure(decimal idTutor)
    {
        await _integracao.ExecutarPetJsonAsync(idTutor);
        return Ok(new { mensagem = "PR_PET_JSON executada. Saida DBMS_OUTPUT permanece no Oracle.", idTutor });
    }

    [HttpGet("api/pets/{petId}/integracao-oracle/idade")]
    public async Task<IActionResult> CalcularIdade(decimal petId)
    {
        var idade = await _integracao.CalcularIdadePetAsync(petId);
        return Ok(new { petId, idadeAnos = idade });
    }
}
