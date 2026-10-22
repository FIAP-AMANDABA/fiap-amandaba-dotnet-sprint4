using Amandaba.Domain.Entities;
namespace Amandaba.API.Application.Interfaces;
public interface ICompartilhamentoUseCase
{
    PetVeterinarioEntity Compartilhar(decimal petId, decimal veterinarioId);
    IEnumerable<PetVeterinarioEntity> Listar(decimal petId);
    bool Revogar(decimal petId, decimal veterinarioId);
    IEnumerable<PetEntity> ListarPetsVeterinario(decimal veterinarioId);
}
