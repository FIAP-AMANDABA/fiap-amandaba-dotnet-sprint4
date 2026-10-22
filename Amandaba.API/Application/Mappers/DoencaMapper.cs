using Amandaba.Application.Dtos.Doencas;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class DoencaMapper
    {
        public static PetDoencaEntity ToEntity(
            this DoencaRequestDto dto,
            decimal idPet)
        {
            return new PetDoencaEntity
            {
                IdPet = idPet,
                Nome = dto.Nome,
                DataDiagnostico = dto.DataDiagnostico,
                Status = dto.Status,
                Tratamento = dto.Tratamento,
                Observacao = dto.Observacao,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this DoencaRequestDto dto,
            PetDoencaEntity entity)
        {
            entity.Nome = dto.Nome;
            entity.DataDiagnostico = dto.DataDiagnostico;
            entity.Status = dto.Status;
            entity.Tratamento = dto.Tratamento;
            entity.Observacao = dto.Observacao;
        }

        public static DoencaResponseDto ToResponseDto(
            this PetDoencaEntity entity)
        {
            return new DoencaResponseDto
            {
                IdRegistroDoenca = entity.IdRegistroDoenca,
                IdPet = entity.IdPet,
                Nome = entity.Nome,
                DataDiagnostico = entity.DataDiagnostico,
                Status = entity.Status,
                Tratamento = entity.Tratamento,
                Observacao = entity.Observacao,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}