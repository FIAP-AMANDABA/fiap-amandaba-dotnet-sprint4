using Amandaba.Domain.Entities;

namespace Amandaba.API.Domain.Interfaces
{
    public interface IPesoRepository
    {
        bool ExistePet(decimal idPet);

        IEnumerable<PetPesoEntity> ObterPorPet(decimal idPet);

        PetPesoEntity? ObterPorId(decimal idPet, decimal pesoId);

        PetPesoEntity? ObterAtual(decimal idPet);

        PetPesoEntity Cadastrar(PetPesoEntity peso);

        void Atualizar(PetPesoEntity peso);

        void Excluir(PetPesoEntity peso);
    }
}