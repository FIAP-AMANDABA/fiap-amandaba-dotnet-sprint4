using Amandaba.Application.Dtos.Medicamentos;

namespace Amandaba.Application.Interfaces
{
    public interface IMedicamentoUseCase
    {
        IEnumerable<MedicamentoResponseDto> ObterPorPet(
            decimal petId,
            string? status);

        MedicamentoResponseDto? ObterPorId(
            decimal petId,
            decimal medicamentoId);

        MedicamentoResponseDto Cadastrar(
            decimal petId,
            MedicamentoRequestDto dto);

        bool Atualizar(
            decimal petId,
            decimal medicamentoId,
            MedicamentoRequestDto dto);

        bool AtualizarStatus(
            decimal petId,
            decimal medicamentoId,
            MedicamentoStatusRequestDto dto);

        bool Excluir(
            decimal petId,
            decimal medicamentoId);
    }
}