using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Consultas;
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
    [Route("api/pets/{petId}/consultas")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaUseCase _consultaUseCase;

        public ConsultasController(
            IConsultaUseCase consultaUseCase)
        {
            _consultaUseCase = consultaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar consultas do pet",
            Description = "Retorna as consultas do pet, com filtro opcional por status."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Consultas retornadas com sucesso."
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
                    _consultaUseCase.ObterPorPet(
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

        [HttpGet("{consultaId}")]
        [SwaggerOperation(
            Summary = "Consultar consulta",
            Description = "Retorna uma consulta específica cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Consulta retornada com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou consulta não encontrada."
        )]
        public IActionResult ObterPorId(
            decimal petId,
            decimal consultaId)
        {
            try
            {
                var consulta = _consultaUseCase.ObterPorId(
                    petId,
                    consultaId
                );

                if (consulta is null)
                {
                    return NotFound(
                        new { mensagem = "Consulta não encontrada." }
                    );
                }

                return Ok(consulta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar consulta",
            Description = "Cadastra uma nova consulta para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            "Consulta cadastrada com sucesso."
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
            [FromBody] ConsultaRequestDto dto)
        {
            try
            {
                var consulta = _consultaUseCase.Cadastrar(
                    petId,
                    dto
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        consultaId = consulta.IdConsulta
                    },
                    consulta
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

        [HttpPut("{consultaId}")]
        [SwaggerOperation(
            Summary = "Editar consulta",
            Description = "Atualiza os dados de uma consulta cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Consulta atualizada com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou consulta não encontrada."
        )]
        public IActionResult Atualizar(
            decimal petId,
            decimal consultaId,
            [FromBody] ConsultaRequestDto dto)
        {
            try
            {
                var atualizado = _consultaUseCase.Atualizar(
                    petId,
                    consultaId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Consulta não encontrada." }
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

        [HttpPatch("{consultaId}/status")]
        [SwaggerOperation(
            Summary = "Alterar status da consulta",
            Description = "Altera o status para AGENDADA, REALIZADA ou CANCELADA."
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
            "Pet ou consulta não encontrada."
        )]
        public IActionResult AtualizarStatus(
            decimal petId,
            decimal consultaId,
            [FromBody] ConsultaStatusRequestDto dto)
        {
            try
            {
                var atualizado =
                    _consultaUseCase.AtualizarStatus(
                        petId,
                        consultaId,
                        dto
                    );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Consulta não encontrada." }
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

        [HttpDelete("{consultaId}")]
        [SwaggerOperation(
            Summary = "Excluir consulta",
            Description = "Remove uma consulta cadastrada para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Consulta excluída com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou consulta não encontrada."
        )]
        public IActionResult Excluir(
            decimal petId,
            decimal consultaId)
        {
            try
            {
                var excluido = _consultaUseCase.Excluir(
                    petId,
                    consultaId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Consulta não encontrada." }
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