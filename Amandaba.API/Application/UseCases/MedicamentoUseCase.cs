using Amandaba.Application.Dtos.Medicamentos;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class MedicamentoUseCase : IMedicamentoUseCase
    {
        private readonly IMedicamentoRepository _medicamentoRepository;

        public MedicamentoUseCase(
            IMedicamentoRepository medicamentoRepository)
        {
            _medicamentoRepository = medicamentoRepository;
        }

        public IEnumerable<MedicamentoResponseDto> ObterPorPet(
            decimal petId,
            string? status)
        {
            ValidarPet(petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = NormalizarEValidarStatus(status);
            }

            return _medicamentoRepository
                .ObterPorPet(petId, status)
                .Select(m => m.ToResponseDto())
                .ToList();
        }

        public MedicamentoResponseDto? ObterPorId(
            decimal petId,
            decimal medicamentoId)
        {
            ValidarPet(petId);

            var medicamento = _medicamentoRepository.ObterPorId(
                petId,
                medicamentoId
            );

            return medicamento?.ToResponseDto();
        }

        public MedicamentoResponseDto Cadastrar(
            decimal petId,
            MedicamentoRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var medicamento = dto.ToEntity(petId);

            medicamento = _medicamentoRepository.Cadastrar(
                medicamento
            );

            return medicamento.ToResponseDto();
        }

        public bool Atualizar(
            decimal petId,
            decimal medicamentoId,
            MedicamentoRequestDto dto)
        {
            ValidarPet(petId);

            var medicamento = _medicamentoRepository.ObterPorId(
                petId,
                medicamentoId
            );

            if (medicamento is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(medicamento);

            _medicamentoRepository.Atualizar(medicamento);

            return true;
        }

        public bool AtualizarStatus(
            decimal petId,
            decimal medicamentoId,
            MedicamentoStatusRequestDto dto)
        {
            ValidarPet(petId);

            var medicamento = _medicamentoRepository.ObterPorId(
                petId,
                medicamentoId
            );

            if (medicamento is null)
                return false;

            medicamento.Status = NormalizarEValidarStatus(
                dto.Status
            );

            _medicamentoRepository.Atualizar(medicamento);

            return true;
        }

        public bool Excluir(
            decimal petId,
            decimal medicamentoId)
        {
            ValidarPet(petId);

            var medicamento = _medicamentoRepository.ObterPorId(
                petId,
                medicamentoId
            );

            if (medicamento is null)
                return false;

            _medicamentoRepository.Excluir(medicamento);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_medicamentoRepository.ExistePet(petId))
            {
                throw new KeyNotFoundException(
                    "Pet não encontrado."
                );
            }
        }

        private static void ValidarDados(
            MedicamentoRequestDto dto)
        {
            dto.Nome = dto.Nome.Trim();

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                throw new ArgumentException(
                    "O nome do medicamento é obrigatório."
                );
            }

            if (dto.Dosagem.HasValue &&
                dto.Dosagem.Value <= 0)
            {
                throw new ArgumentException(
                    "A dosagem deve ser maior que zero."
                );
            }

            if (dto.DataTermino.HasValue &&
                dto.DataTermino.Value.Date < dto.DataInicio.Date)
            {
                throw new ArgumentException(
                    "A data de término não pode ser anterior à data de início."
                );
            }

            dto.Status = NormalizarEValidarStatus(
                dto.Status
            );
        }

        private static string NormalizarEValidarStatus(
            string status)
        {
            status = status.Trim().ToUpper();

            if (status != "EM_USO" &&
                status != "CONCLUIDO" &&
                status != "SUSPENSO")
            {
                throw new ArgumentException(
                    "Status inválido. Utilize EM_USO, CONCLUIDO ou SUSPENSO."
                );
            }

            return status;
        }
    }
}