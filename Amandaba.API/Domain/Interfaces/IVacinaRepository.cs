using Amandaba.Domain.Entities;

namespace Amandaba.Domain.Interfaces
{
    public interface IVacinaRepository
    {
        IEnumerable<VacinaEntity> ObterCatalogo();

        VacinaEntity? ObterVacinaPorId(decimal vacinaId);

        bool ExistePet(decimal petId);

        IEnumerable<PetVacinaEntity> ObterAplicacoesPorPet(decimal petId);

        PetVacinaEntity? ObterAplicacaoPorId(
            decimal petId,
            decimal aplicacaoId);

        PetVacinaEntity CadastrarAplicacao(PetVacinaEntity aplicacao);

        void AtualizarAplicacao(PetVacinaEntity aplicacao);

        void ExcluirAplicacao(PetVacinaEntity aplicacao);
    }
}