using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IExameRepository
    {
        bool ExistePet(decimal petId);

        IEnumerable<ExameEntity> ObterPorPet(
            decimal petId,
            string? status);

        ExameEntity? ObterPorId(
            decimal petId,
            decimal exameId);

        ExameEntity Cadastrar(ExameEntity exame);

        void Atualizar(ExameEntity exame);

        void Excluir(ExameEntity exame);
    }
}