using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Exames;
using Amandaba.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Amandaba.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/pets/{petId}/exames")]
    public class ExamesController : ControllerBase
    {
        private readonly IExameUseCase _exameUseCase;

        public ExamesController(
            IExameUseCase exameUseCase)
        {
            _exameUseCase = exameUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar exames do pet",
            Description = "Retorna os exames do pet, com filtro opcional por status."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Exames retornados com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Status inválido."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult ObterPorPet(
            decimal petId,
            [FromQuery] string? status)
        {
            try
            {
                return Ok(
                    _exameUseCase.ObterPorPet(
                        petId,
                        status
                    )
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

        [HttpGet("{exameId}")]
        [SwaggerOperation(
            Summary = "Consultar exame",
            Description = "Retorna um exame específico cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Exame retornado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou exame não encontrado."
        )]
        public IActionResult ObterPorId(
            decimal petId,
            decimal exameId)
        {
            try
            {
                var exame = _exameUseCase.ObterPorId(
                    petId,
                    exameId
                );

                if (exame is null)
                {
                    return NotFound(
                        new { mensagem = "Exame não encontrado." }
                    );
                }

                return Ok(exame);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar exame",
            Description = "Cadastra um novo exame para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            "Exame cadastrado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult Cadastrar(
            decimal petId,
            [FromBody] ExameRequestDto dto)
        {
            try
            {
                var exame = _exameUseCase.Cadastrar(
                    petId,
                    dto
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        exameId = exame.IdExame
                    },
                    exame
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

        [HttpPut("{exameId}")]
        [SwaggerOperation(
            Summary = "Editar exame",
            Description = "Atualiza os dados de um exame cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Exame atualizado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou exame não encontrado."
        )]
        public IActionResult Atualizar(
            decimal petId,
            decimal exameId,
            [FromBody] ExameRequestDto dto)
        {
            try
            {
                var atualizado = _exameUseCase.Atualizar(
                    petId,
                    exameId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Exame não encontrado." }
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

        [HttpPatch("{exameId}/status")]
        [SwaggerOperation(
            Summary = "Alterar status do exame",
            Description = "Altera o status para SOLICITADO, REALIZADO ou RESULTADO_DISPONIVEL."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Status atualizado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Status inválido."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou exame não encontrado."
        )]
        public IActionResult AtualizarStatus(
            decimal petId,
            decimal exameId,
            [FromBody] ExameStatusRequestDto dto)
        {
            try
            {
                var atualizado = _exameUseCase.AtualizarStatus(
                    petId,
                    exameId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Exame não encontrado." }
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

        [HttpDelete("{exameId}")]
        [SwaggerOperation(
            Summary = "Excluir exame",
            Description = "Remove um exame cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Exame excluído com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou exame não encontrado."
        )]
        public IActionResult Excluir(
            decimal petId,
            decimal exameId)
        {
            try
            {
                var excluido = _exameUseCase.Excluir(
                    petId,
                    exameId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Exame não encontrado." }
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