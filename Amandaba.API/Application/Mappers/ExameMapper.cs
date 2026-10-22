using Amandaba.Application.Dtos.Exames;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class ExameMapper
    {
        public static ExameEntity ToEntity(
            this ExameRequestDto dto,
            decimal idPet)
        {
            return new ExameEntity
            {
                IdPet = idPet,
                Nome = dto.Nome,
                DataSolicitacao = dto.DataSolicitacao,
                DataRealizacao = dto.DataRealizacao,
                Veterinario = dto.Veterinario,
                Clinica = dto.Clinica,
                Motivo = dto.Motivo,
                Resultado = dto.Resultado,
                Observacao = dto.Observacao,
                Arquivo = dto.Arquivo,
                Status = dto.Status,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this ExameRequestDto dto,
            ExameEntity entity)
        {
            entity.Nome = dto.Nome;
            entity.DataSolicitacao = dto.DataSolicitacao;
            entity.DataRealizacao = dto.DataRealizacao;
            entity.Veterinario = dto.Veterinario;
            entity.Clinica = dto.Clinica;
            entity.Motivo = dto.Motivo;
            entity.Resultado = dto.Resultado;
            entity.Observacao = dto.Observacao;
            entity.Arquivo = dto.Arquivo;
            entity.Status = dto.Status;
        }

        public static ExameResponseDto ToResponseDto(
            this ExameEntity entity)
        {
            return new ExameResponseDto
            {
                IdExame = entity.IdExame,
                IdPet = entity.IdPet,
                Nome = entity.Nome,
                DataSolicitacao = entity.DataSolicitacao,
                DataRealizacao = entity.DataRealizacao,
                Veterinario = entity.Veterinario,
                Clinica = entity.Clinica,
                Motivo = entity.Motivo,
                Resultado = entity.Resultado,
                Observacao = entity.Observacao,
                Arquivo = entity.Arquivo,
                Status = entity.Status,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}