using Amandaba.Application.Dtos.Exames;

namespace Amandaba.Application.Interfaces
{
    public interface IExameUseCase
    {
        IEnumerable<ExameResponseDto> ObterPorPet(
            decimal petId,
            string? status);

        ExameResponseDto? ObterPorId(
            decimal petId,
            decimal exameId);

        ExameResponseDto Cadastrar(
            decimal petId,
            ExameRequestDto dto);

        bool Atualizar(
            decimal petId,
            decimal exameId,
            ExameRequestDto dto);

        bool AtualizarStatus(
            decimal petId,
            decimal exameId,
            ExameStatusRequestDto dto);

        bool Excluir(
            decimal petId,
            decimal exameId);
    }
}