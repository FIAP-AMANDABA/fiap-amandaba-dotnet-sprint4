using Microsoft.AspNetCore.Authorization;
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
    [Route("api/especies")]
    public class EspeciesController : ControllerBase
    {
        private readonly IEspecieUseCase _especieUseCase;

        public EspeciesController(IEspecieUseCase especieUseCase)
        {
            _especieUseCase = especieUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar espécies",
            Description = "Retorna todas as espécies disponíveis para cadastro de pets."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Espécies retornadas com sucesso."
        )]
        public IActionResult ObterTodas()
        {
            return Ok(_especieUseCase.ObterTodas());
        }
    }
}