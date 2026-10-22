using Amandaba.Application.Dtos.Consultas;

namespace Amandaba.Application.Interfaces
{
    public interface IConsultaUseCase
    {
        IEnumerable<ConsultaResponseDto> ObterPorPet(
            decimal petId,
            string? status);

        ConsultaResponseDto? ObterPorId(
            decimal petId,
            decimal consultaId);

        ConsultaResponseDto Cadastrar(
            decimal petId,
            ConsultaRequestDto dto);

        bool Atualizar(
            decimal petId,
            decimal consultaId,
            ConsultaRequestDto dto);

        bool AtualizarStatus(
            decimal petId,
            decimal consultaId,
            ConsultaStatusRequestDto dto);

        bool Excluir(
            decimal petId,
            decimal consultaId);
    }
}