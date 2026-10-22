using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IDoencaRepository
    {
        bool ExistePet(decimal petId);

        IEnumerable<PetDoencaEntity> ObterPorPet(decimal petId);

        PetDoencaEntity? ObterPorId(
            decimal petId,
            decimal doencaId);

        PetDoencaEntity Cadastrar(PetDoencaEntity doenca);

        void Atualizar(PetDoencaEntity doenca);

        void Excluir(PetDoencaEntity doenca);
    }
}