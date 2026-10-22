using Amandaba.API.Application.Dtos.Alergias;
using Amandaba.Application.Dtos.Alergias;

namespace Amandaba.Application.Interfaces
{
    public interface IAlergiaUseCase
    {
        IEnumerable<AlergiaResponseDto> ObterPorPet(decimal petId);

        AlergiaResponseDto? ObterPorId(
            decimal petId,
            decimal alergiaId);

        AlergiaResponseDto Cadastrar(
            decimal petId,
            AlergiaRequestDto dto);

        bool Atualizar(
            decimal petId,
            decimal alergiaId,
            AlergiaRequestDto dto);

        bool Excluir(
            decimal petId,
            decimal alergiaId);
    }
}