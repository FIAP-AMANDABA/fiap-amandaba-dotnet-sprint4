using Amandaba.Application.Dtos.Pets;

namespace Amandaba.Application.Interfaces
{
    public interface IPetUseCase
    {
        IEnumerable<PetResponseDto> ObterPorTutor(decimal idTutor);
        Amandaba.API.Application.Dtos.PageResponseDto<PetResponseDto> ObterPorTutorPaginado(
            decimal idTutor, int pagina, int tamanho, string? nome, string? ordem);

        PetResponseDto? ObterPorId(decimal idPet);

        PetResponseDto Cadastrar(decimal idTutor, PetRequestDto dto);

        bool Atualizar(decimal idPet, PetRequestDto dto);

        bool AtualizarStatus(decimal idPet, PetStatusRequestDto dto);
    }
}