namespace Amandaba.API.Application.Interfaces;

public interface IPetAccessService
{
    decimal? ObterIdUsuario(System.Security.Claims.ClaimsPrincipal usuario);
    decimal? ObterIdTutor(decimal idUsuario);
    decimal? ObterIdVeterinario(decimal idUsuario);
    bool ExistePet(decimal petId);
    bool PodeAcessarPet(decimal idUsuario, decimal idPet, bool somenteTutor, string perfil);
}
