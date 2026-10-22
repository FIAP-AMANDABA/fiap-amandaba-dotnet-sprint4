using Amandaba.Application.Dtos.Vacinas;

namespace Amandaba.API.Application.Interfaces
{
    public interface IVacinaUseCase
    {
        IEnumerable<VacinaCatalogoResponseDto> ObterCatalogo();

        VacinaCatalogoResponseDto? ObterVacinaPorId(decimal vacinaId);

        IEnumerable<VacinaAplicacaoResponseDto> ObterAplicacoesPorPet(decimal petId);

        VacinaAplicacaoResponseDto? ObterAplicacaoPorId(
            decimal petId,
            decimal aplicacaoId);

        VacinaAplicacaoResponseDto CadastrarAplicacao(
            decimal petId,
            VacinaAplicacaoRequestDto dto);

        bool AtualizarAplicacao(
            decimal petId,
            decimal aplicacaoId,
            VacinaAplicacaoRequestDto dto);

        bool ExcluirAplicacao(
            decimal petId,
            decimal aplicacaoId);
    }
}