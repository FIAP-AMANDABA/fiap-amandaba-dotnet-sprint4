using Amandaba.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Amandaba.API.Presentation.Filters;

// Filtro MVC simples: protege as rotas legadas sem mudar seus contratos.
public class PetAccessFilter : IAsyncActionFilter
{
    private readonly IPetAccessService _acesso;
    public PetAccessFilter(IPetAccessService acesso) { _acesso = acesso; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var req = context.HttpContext.Request;
        var rota = req.RouteValues;
        decimal petId = 0;
        bool rotaDePet = rota.TryGetValue("petId", out var petObj) &&
                         decimal.TryParse(petObj?.ToString(), out petId);
        decimal tutorId = 0;
        bool rotaDeTutor = rota.TryGetValue("idTutor", out var tutorObj) &&
                           decimal.TryParse(tutorObj?.ToString(), out tutorId);
        // Rotas publicas de monitoramento nao exigem usuario ativo.
        if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        { await next(); return; }
        var userId = _acesso.ObterIdUsuario(context.HttpContext.User);
        if (!userId.HasValue) { context.Result = new UnauthorizedObjectResult(new { mensagem = "JWT sem userId valido." }); return; }
        if (!_acesso.UsuarioAtivo(userId.Value))
        {
            context.Result = new ObjectResult(new { mensagem = "Usuario inativo ou inexistente." }) { StatusCode = 403 };
            return;
        }
        if (!rotaDePet && !rotaDeTutor) { await next(); return; }
        var perfil = context.HttpContext.User.FindFirst("perfil")?.Value ?? "";
        if (rotaDeTutor)
        {
            if (perfil != "TUTOR" || _acesso.ObterIdTutor(userId.Value) != tutorId)
            {
                context.Result = new ObjectResult(new { mensagem = "Tutor nao autorizado." }) { StatusCode = 403 };
                return;
            }
        }
        if (rotaDePet)
        {
            if (!_acesso.ExistePet(petId))
            {
                context.Result = new NotFoundObjectResult(new { mensagem = "Pet nao encontrado." });
                return;
            }
            // Veterinarios podem fazer PUT/PATCH nos registros de SAUDE, mas nao no cadastro do pet.
            var caminho = req.Path.Value?.TrimEnd('/') ?? "";
            var edicaoCadastro = (req.Method == "PUT" &&
                    caminho.Equals($"/api/pets/{petId}", StringComparison.OrdinalIgnoreCase)) ||
                (req.Method == "PATCH" &&
                    caminho.Equals($"/api/pets/{petId}/status", StringComparison.OrdinalIgnoreCase));
            var somenteTutor = edicaoCadastro ||
                caminho.Contains("/veterinarios", StringComparison.OrdinalIgnoreCase) ||
                caminho.EndsWith("/pesos/sincronizar", StringComparison.OrdinalIgnoreCase);
            if (!_acesso.PodeAcessarPet(userId.Value, petId, somenteTutor, perfil))
            {
                context.Result = new ObjectResult(new { mensagem = "Sem permissao para este pet." }) { StatusCode = 403 };
                return;
            }
        }
        await next();
    }
}
