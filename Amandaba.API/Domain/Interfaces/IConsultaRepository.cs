using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IConsultaRepository
    {
        bool ExistePet(decimal petId);

        IEnumerable<ConsultaEntity> ObterPorPet(
            decimal petId,
            string? status);

        ConsultaEntity? ObterPorId(
            decimal petId,
            decimal consultaId);

        ConsultaEntity Cadastrar(ConsultaEntity consulta);

        void Atualizar(ConsultaEntity consulta);

        void Excluir(ConsultaEntity consulta);
    }
}