using Amandaba.Domain.Entities;
namespace Amandaba.API.Domain.Interfaces;
public interface ICompartilhamentoRepository
{
    VeterinarioEntity? BuscarVeterinario(decimal id);
    PetVeterinarioEntity? BuscarVinculo(decimal petId, decimal veterinarioId);
    IEnumerable<PetVeterinarioEntity> ListarPorPet(decimal petId);
    IEnumerable<PetEntity> ListarPetsVeterinario(decimal veterinarioId);
    void Salvar(PetVeterinarioEntity vinculo);
}
