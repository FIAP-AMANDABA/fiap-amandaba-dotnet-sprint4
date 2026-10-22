using Amandaba.Domain.Entities;

namespace Amandaba.API.Domain.Interfaces
{
    public interface IPetRepository
    {
        IEnumerable<PetEntity> ObterPorTutor(decimal idTutor);

        PetEntity? ObterPorId(decimal idPet);

        PetEntity Cadastrar(PetEntity pet);

        void Atualizar(PetEntity pet);

        bool ExisteTutor(decimal idTutor);

        bool ExisteEspecie(decimal idEspecie);
    }
}