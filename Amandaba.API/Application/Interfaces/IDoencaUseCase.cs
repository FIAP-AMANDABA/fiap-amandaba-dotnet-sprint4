using Amandaba.Application.Dtos.Doencas;

namespace Amandaba.Application.Interfaces
{
    public interface IDoencaUseCase
    {
        IEnumerable<DoencaResponseDto> ObterPorPet(decimal petId);

        DoencaResponseDto? ObterPorId(
            decimal petId,
            decimal doencaId);

        DoencaResponseDto Cadastrar(
            decimal petId,
            DoencaRequestDto dto);

        bool Atualizar(
            decimal petId,
            decimal doencaId,
            DoencaRequestDto dto);

        bool Excluir(
            decimal petId,
            decimal doencaId);
    }
}