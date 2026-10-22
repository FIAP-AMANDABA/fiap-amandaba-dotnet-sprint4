using Amandaba.API.Application.Dtos.Alergias;
using Amandaba.Application.Dtos.Alergias;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class AlergiaMapper
    {
        public static PetAlergiaEntity ToEntity(
            this AlergiaRequestDto dto,
            decimal idPet)
        {
            return new PetAlergiaEntity
            {
                IdPet = idPet,
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                DataIdentificacao = dto.DataIdentificacao,
                Reacao = dto.Reacao,
                Gravidade = dto.Gravidade,
                Observacao = dto.Observacao,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this AlergiaRequestDto dto,
            PetAlergiaEntity entity)
        {
            entity.Nome = dto.Nome;
            entity.Tipo = dto.Tipo;
            entity.DataIdentificacao = dto.DataIdentificacao;
            entity.Reacao = dto.Reacao;
            entity.Gravidade = dto.Gravidade;
            entity.Observacao = dto.Observacao;
        }

        public static AlergiaResponseDto ToResponseDto(
            this PetAlergiaEntity entity)
        {
            return new AlergiaResponseDto
            {
                IdRegistroAlergia = entity.IdRegistroAlergia,
                IdPet = entity.IdPet,
                Nome = entity.Nome,
                Tipo = entity.Tipo,
                DataIdentificacao = entity.DataIdentificacao,
                Reacao = entity.Reacao,
                Gravidade = entity.Gravidade,
                Observacao = entity.Observacao,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}