using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IAlergiaRepository
    {
        bool ExistePet(decimal petId);

        IEnumerable<PetAlergiaEntity> ObterPorPet(decimal petId);

        PetAlergiaEntity? ObterPorId(
            decimal petId,
            decimal alergiaId);

        PetAlergiaEntity Cadastrar(PetAlergiaEntity alergia);

        void Atualizar(PetAlergiaEntity alergia);

        void Excluir(PetAlergiaEntity alergia);
    }
}