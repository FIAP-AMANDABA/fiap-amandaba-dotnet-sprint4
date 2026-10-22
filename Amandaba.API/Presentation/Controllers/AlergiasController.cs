using Microsoft.AspNetCore.Authorization;
using Amandaba.API.Application.Dtos.Alergias;
using Amandaba.Application.Dtos.Alergias;
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
    [Route("api/pets/{petId}/alergias")]
    public class AlergiasController : ControllerBase
    {
        private readonly IAlergiaUseCase _alergiaUseCase;

        public AlergiasController(IAlergiaUseCase alergiaUseCase)
        {
            _alergiaUseCase = alergiaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar alergias do pet",
            Description = "Retorna todas as alergias cadastradas para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Alergias retornadas com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet não encontrado.")]
        public IActionResult ObterPorPet(decimal petId)
        {
            try
            {
                return Ok(_alergiaUseCase.ObterPorPet(petId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("{alergiaId}")]
        [SwaggerOperation(
            Summary = "Consultar alergia",
            Description = "Retorna uma alergia específica cadastrada para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Alergia retornada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou alergia não encontrada.")]
        public IActionResult ObterPorId(
            decimal petId,
            decimal alergiaId)
        {
            try
            {
                var alergia = _alergiaUseCase.ObterPorId(
                    petId,
                    alergiaId
                );

                if (alergia is null)
                {
                    return NotFound(
                        new { mensagem = "Alergia não encontrada." }
                    );
                }

                return Ok(alergia);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar alergia",
            Description = "Cadastra uma nova alergia para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Alergia cadastrada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet não encontrado.")]
        public IActionResult Cadastrar(
            decimal petId,
            [FromBody] AlergiaRequestDto dto)
        {
            try
            {
                var alergia = _alergiaUseCase.Cadastrar(
                    petId,
                    dto
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        alergiaId = alergia.IdRegistroAlergia
                    },
                    alergia
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

        [HttpPut("{alergiaId}")]
        [SwaggerOperation(
            Summary = "Editar alergia",
            Description = "Atualiza os dados de uma alergia cadastrada para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Alergia atualizada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou alergia não encontrada.")]
        public IActionResult Atualizar(
            decimal petId,
            decimal alergiaId,
            [FromBody] AlergiaRequestDto dto)
        {
            try
            {
                var atualizado = _alergiaUseCase.Atualizar(
                    petId,
                    alergiaId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Alergia não encontrada." }
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

        [HttpDelete("{alergiaId}")]
        [SwaggerOperation(
            Summary = "Excluir alergia",
            Description = "Remove uma alergia cadastrada para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Alergia excluída com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou alergia não encontrada.")]
        public IActionResult Excluir(
            decimal petId,
            decimal alergiaId)
        {
            try
            {
                var excluido = _alergiaUseCase.Excluir(
                    petId,
                    alergiaId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Alergia não encontrada." }
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