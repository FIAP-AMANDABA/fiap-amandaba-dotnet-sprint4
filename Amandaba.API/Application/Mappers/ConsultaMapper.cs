using Amandaba.Application.Dtos.Consultas;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class ConsultaMapper
    {
        public static ConsultaEntity ToEntity(
            this ConsultaRequestDto dto,
            decimal idPet)
        {
            return new ConsultaEntity
            {
                IdPet = idPet,
                DataConsulta = dto.DataConsulta,
                Horario = dto.Horario,
                Veterinario = dto.Veterinario,
                Clinica = dto.Clinica,
                Motivo = dto.Motivo,
                Sintomas = dto.Sintomas,
                Peso = dto.Peso,
                Diagnostico = dto.Diagnostico,
                Tratamento = dto.Tratamento,
                Observacao = dto.Observacao,
                Retorno = dto.Retorno,
                DataRetorno = dto.DataRetorno,
                Status = dto.Status,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this ConsultaRequestDto dto,
            ConsultaEntity entity)
        {
            entity.DataConsulta = dto.DataConsulta;
            entity.Horario = dto.Horario;
            entity.Veterinario = dto.Veterinario;
            entity.Clinica = dto.Clinica;
            entity.Motivo = dto.Motivo;
            entity.Sintomas = dto.Sintomas;
            entity.Peso = dto.Peso;
            entity.Diagnostico = dto.Diagnostico;
            entity.Tratamento = dto.Tratamento;
            entity.Observacao = dto.Observacao;
            entity.Retorno = dto.Retorno;
            entity.DataRetorno = dto.DataRetorno;
            entity.Status = dto.Status;
        }

        public static ConsultaResponseDto ToResponseDto(
            this ConsultaEntity entity)
        {
            return new ConsultaResponseDto
            {
                IdConsulta = entity.IdConsulta,
                IdPet = entity.IdPet,
                DataConsulta = entity.DataConsulta,
                Horario = entity.Horario,
                Veterinario = entity.Veterinario,
                Clinica = entity.Clinica,
                Motivo = entity.Motivo,
                Sintomas = entity.Sintomas,
                Peso = entity.Peso,
                Diagnostico = entity.Diagnostico,
                Tratamento = entity.Tratamento,
                Observacao = entity.Observacao,
                Retorno = entity.Retorno,
                DataRetorno = entity.DataRetorno,
                Status = entity.Status,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}