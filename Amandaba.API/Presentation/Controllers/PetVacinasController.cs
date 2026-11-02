using Amandaba.API.Presentation;
using Microsoft.AspNetCore.Authorization;
using Amandaba.API.Application.Interfaces;
using Amandaba.Application.Dtos.Vacinas;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Amandaba.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/pets/{petId}/vacinas")]
    public class PetVacinasController : ControllerBase
    {
        private readonly IVacinaUseCase _vacinaUseCase;

        public PetVacinasController(IVacinaUseCase vacinaUseCase)
        {
            _vacinaUseCase = vacinaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar vacinas aplicadas no pet")]
        public IActionResult ObterPorPet(decimal petId)
        {
            try
            {
                var registros = _vacinaUseCase.ObterAplicacoesPorPet(petId).ToList();
                foreach (var a in registros)
                    a.Links = HateoasLinks.ParaRegistro($"/api/pets/{petId}/vacinas/{a.IdAplicacaoVacina}");
                return Ok(registros);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("{aplicacaoId}")]
        [SwaggerOperation(Summary = "Consultar aplicação de vacina")]
        public IActionResult ObterPorId(decimal petId, decimal aplicacaoId)
        {
            try
            {
                var aplicacao = _vacinaUseCase
                    .ObterAplicacaoPorId(petId, aplicacaoId);

                if (aplicacao is null)
                {
                    return NotFound(
                        new { mensagem = "Aplicação de vacina não encontrada." }
                    );
                }

                aplicacao.Links = HateoasLinks.ParaRegistro($"/api/pets/{petId}/vacinas/{aplicacaoId}");
                return Ok(aplicacao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Registrar aplicação de vacina")]
        public IActionResult Cadastrar(
            decimal petId,
            [FromBody] VacinaAplicacaoRequestDto dto)
        {
            try
            {
                var aplicacao = _vacinaUseCase
                    .CadastrarAplicacao(petId, dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        aplicacaoId = aplicacao.IdAplicacaoVacina
                    },
                    aplicacao
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpPut("{aplicacaoId}")]
        [SwaggerOperation(Summary = "Editar aplicação de vacina")]
        public IActionResult Atualizar(
            decimal petId,
            decimal aplicacaoId,
            [FromBody] VacinaAplicacaoRequestDto dto)
        {
            try
            {
                var atualizado = _vacinaUseCase.AtualizarAplicacao(
                    petId,
                    aplicacaoId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Aplicação de vacina não encontrada." }
                    );
                }

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpDelete("{aplicacaoId}")]
        [SwaggerOperation(Summary = "Excluir aplicação de vacina")]
        public IActionResult Excluir(
            decimal petId,
            decimal aplicacaoId)
        {
            try
            {
                var excluido = _vacinaUseCase.ExcluirAplicacao(
                    petId,
                    aplicacaoId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Aplicação de vacina não encontrada." }
                    );
                }

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }
    }
}