using Amandaba.Application.Dtos.Medicamentos;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class MedicamentoMapper
    {
        public static PetMedicamentoEntity ToEntity(
            this MedicamentoRequestDto dto,
            decimal idPet)
        {
            return new PetMedicamentoEntity
            {
                IdPet = idPet,
                Nome = dto.Nome,
                Motivo = dto.Motivo,
                Dosagem = dto.Dosagem,
                Unidade = dto.Unidade,
                Quantidade = dto.Quantidade,
                Frequencia = dto.Frequencia,
                Administracao = dto.Administracao,
                Horario = dto.Horario,
                DataInicio = dto.DataInicio,
                DataTermino = dto.DataTermino,
                Status = dto.Status,
                Prescricao = dto.Prescricao,
                Observacao = dto.Observacao,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this MedicamentoRequestDto dto,
            PetMedicamentoEntity entity)
        {
            entity.Nome = dto.Nome;
            entity.Motivo = dto.Motivo;
            entity.Dosagem = dto.Dosagem;
            entity.Unidade = dto.Unidade;
            entity.Quantidade = dto.Quantidade;
            entity.Frequencia = dto.Frequencia;
            entity.Administracao = dto.Administracao;
            entity.Horario = dto.Horario;
            entity.DataInicio = dto.DataInicio;
            entity.DataTermino = dto.DataTermino;
            entity.Status = dto.Status;
            entity.Prescricao = dto.Prescricao;
            entity.Observacao = dto.Observacao;
        }

        public static MedicamentoResponseDto ToResponseDto(
            this PetMedicamentoEntity entity)
        {
            return new MedicamentoResponseDto
            {
                IdRegistroMedicamento = entity.IdRegistroMedicamento,
                IdPet = entity.IdPet,
                Nome = entity.Nome,
                Motivo = entity.Motivo,
                Dosagem = entity.Dosagem,
                Unidade = entity.Unidade,
                Quantidade = entity.Quantidade,
                Frequencia = entity.Frequencia,
                Administracao = entity.Administracao,
                Horario = entity.Horario,
                DataInicio = entity.DataInicio,
                DataTermino = entity.DataTermino,
                Status = entity.Status,
                Prescricao = entity.Prescricao,
                Observacao = entity.Observacao,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}