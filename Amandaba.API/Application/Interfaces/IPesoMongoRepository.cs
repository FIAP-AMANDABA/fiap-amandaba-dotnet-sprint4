using Amandaba.API.Application.Dtos.Pesos;
namespace Amandaba.API.Application.Interfaces;

public interface IPesoMongoRepository
{
    PesoHistoricoDto? ObterPorPet(decimal petId);
    void Salvar(PesoHistoricoDto documento);
}
