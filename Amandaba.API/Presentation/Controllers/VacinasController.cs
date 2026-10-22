using Microsoft.AspNetCore.Authorization;
using Amandaba.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Amandaba.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/vacinas")]
    public class VacinasController : ControllerBase
    {
        private readonly IVacinaUseCase _vacinaUseCase;

        public VacinasController(IVacinaUseCase vacinaUseCase)
        {
            _vacinaUseCase = vacinaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar catálogo de vacinas")]
        public IActionResult ObterCatalogo()
        {
            return Ok(_vacinaUseCase.ObterCatalogo());
        }

        [HttpGet("{vacinaId}")]
        [SwaggerOperation(Summary = "Consultar vacina do catálogo")]
        public IActionResult ObterPorId(decimal vacinaId)
        {
            var vacina = _vacinaUseCase.ObterVacinaPorId(vacinaId);

            if (vacina is null)
                return NotFound(new { mensagem = "Vacina não encontrada." });

            return Ok(vacina);
        }
    }
}