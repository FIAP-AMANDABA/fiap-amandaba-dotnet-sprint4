using Amandaba.Application.Dtos.Pesos;

namespace Amandaba.Application.Interfaces
{
    public interface IPesoUseCase
    {
        IEnumerable<PesoResponseDto> ObterPorPet(decimal idPet);

        PesoResponseDto? ObterAtual(decimal idPet);

        PesoResponseDto Cadastrar(decimal idPet, PesoRequestDto dto);

        bool Atualizar(decimal idPet, decimal pesoId, PesoRequestDto dto);

        bool Excluir(decimal idPet, decimal pesoId);
    }
}