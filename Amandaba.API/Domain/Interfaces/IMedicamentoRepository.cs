using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IMedicamentoRepository
    {
        bool ExistePet(decimal petId);

        IEnumerable<PetMedicamentoEntity> ObterPorPet(
            decimal petId,
            string? status);

        PetMedicamentoEntity? ObterPorId(
            decimal petId,
            decimal medicamentoId);

        PetMedicamentoEntity Cadastrar(
            PetMedicamentoEntity medicamento);

        void Atualizar(PetMedicamentoEntity medicamento);

        void Excluir(PetMedicamentoEntity medicamento);
    }
}