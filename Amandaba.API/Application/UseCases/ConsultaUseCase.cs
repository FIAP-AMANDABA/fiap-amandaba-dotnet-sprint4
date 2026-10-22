using Amandaba.Application.Dtos.Consultas;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class ConsultaUseCase : IConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;

        public ConsultaUseCase(
            IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public IEnumerable<ConsultaResponseDto> ObterPorPet(
            decimal petId,
            string? status)
        {
            ValidarPet(petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = NormalizarEValidarStatus(status);
            }

            return _consultaRepository
                .ObterPorPet(petId, status)
                .Select(c => c.ToResponseDto())
                .ToList();
        }

        public ConsultaResponseDto? ObterPorId(
            decimal petId,
            decimal consultaId)
        {
            ValidarPet(petId);

            var consulta = _consultaRepository.ObterPorId(
                petId,
                consultaId
            );

            return consulta?.ToResponseDto();
        }

        public ConsultaResponseDto Cadastrar(
            decimal petId,
            ConsultaRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var consulta = dto.ToEntity(petId);

            consulta = _consultaRepository.Cadastrar(consulta);

            return consulta.ToResponseDto();
        }

        public bool Atualizar(
            decimal petId,
            decimal consultaId,
            ConsultaRequestDto dto)
        {
            ValidarPet(petId);

            var consulta = _consultaRepository.ObterPorId(
                petId,
                consultaId
            );

            if (consulta is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(consulta);

            _consultaRepository.Atualizar(consulta);

            return true;
        }

        public bool AtualizarStatus(
            decimal petId,
            decimal consultaId,
            ConsultaStatusRequestDto dto)
        {
            ValidarPet(petId);

            var consulta = _consultaRepository.ObterPorId(
                petId,
                consultaId
            );

            if (consulta is null)
                return false;

            consulta.Status = NormalizarEValidarStatus(
                dto.Status
            );

            _consultaRepository.Atualizar(consulta);

            return true;
        }

        public bool Excluir(
            decimal petId,
            decimal consultaId)
        {
            ValidarPet(petId);

            var consulta = _consultaRepository.ObterPorId(
                petId,
                consultaId
            );

            if (consulta is null)
                return false;

            _consultaRepository.Excluir(consulta);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_consultaRepository.ExistePet(petId))
            {
                throw new KeyNotFoundException(
                    "Pet não encontrado."
                );
            }
        }

        private static void ValidarDados(
            ConsultaRequestDto dto)
        {
            if (dto.Peso.HasValue &&
                dto.Peso.Value <= 0)
            {
                throw new ArgumentException(
                    "O peso deve ser maior que zero."
                );
            }

            if (!dto.Retorno &&
                dto.DataRetorno.HasValue)
            {
                throw new ArgumentException(
                    "A data de retorno só pode ser informada quando houver necessidade de retorno."
                );
            }

            if (dto.Retorno &&
                dto.DataRetorno.HasValue &&
                dto.DataRetorno.Value.Date < dto.DataConsulta.Date)
            {
                throw new ArgumentException(
                    "A data de retorno não pode ser anterior à data da consulta."
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

            if (status != "AGENDADA" &&
                status != "REALIZADA" &&
                status != "CANCELADA")
            {
                throw new ArgumentException(
                    "Status inválido. Utilize AGENDADA, REALIZADA ou CANCELADA."
                );
            }

            return status;
        }
    }
}