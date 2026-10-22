using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Doencas;
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
    [Route("api/pets/{petId}/doencas")]
    public class DoencasController : ControllerBase
    {
        private readonly IDoencaUseCase _doencaUseCase;

        public DoencasController(IDoencaUseCase doencaUseCase)
        {
            _doencaUseCase = doencaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar doenças do pet",
            Description = "Retorna todas as doenças e condições de saúde cadastradas para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Doenças retornadas com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult ObterPorPet(decimal petId)
        {
            try
            {
                return Ok(_doencaUseCase.ObterPorPet(petId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("{doencaId}")]
        [SwaggerOperation(
            Summary = "Consultar doença",
            Description = "Retorna uma doença específica cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Doença retornada com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou doença não encontrada."
        )]
        public IActionResult ObterPorId(
            decimal petId,
            decimal doencaId)
        {
            try
            {
                var doenca = _doencaUseCase.ObterPorId(
                    petId,
                    doencaId
                );

                if (doenca is null)
                {
                    return NotFound(
                        new { mensagem = "Doença não encontrada." }
                    );
                }

                return Ok(doenca);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar doença",
            Description = "Cadastra uma doença ou condição de saúde para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            "Doença cadastrada com sucesso."
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
            [FromBody] DoencaRequestDto dto)
        {
            try
            {
                var doenca = _doencaUseCase.Cadastrar(
                    petId,
                    dto
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        doencaId = doenca.IdRegistroDoenca
                    },
                    doenca
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

        [HttpPut("{doencaId}")]
        [SwaggerOperation(
            Summary = "Editar doença",
            Description = "Atualiza os dados de uma doença cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Doença atualizada com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou doença não encontrada."
        )]
        public IActionResult Atualizar(
            decimal petId,
            decimal doencaId,
            [FromBody] DoencaRequestDto dto)
        {
            try
            {
                var atualizado = _doencaUseCase.Atualizar(
                    petId,
                    doencaId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Doença não encontrada." }
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

        [HttpDelete("{doencaId}")]
        [SwaggerOperation(
            Summary = "Excluir doença",
            Description = "Remove uma doença cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Doença excluída com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou doença não encontrada."
        )]
        public IActionResult Excluir(
            decimal petId,
            decimal doencaId)
        {
            try
            {
                var excluido = _doencaUseCase.Excluir(
                    petId,
                    doencaId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Doença não encontrada." }
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