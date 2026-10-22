using Amandaba.API.Infrastructure.Mongo;
namespace Amandaba.API.Application.Interfaces;
public interface IPesoMongoRepository
{
    PesoHistoricoDocumento? ObterPorPet(decimal petId);
    void Salvar(PesoHistoricoDocumento documento);
}
