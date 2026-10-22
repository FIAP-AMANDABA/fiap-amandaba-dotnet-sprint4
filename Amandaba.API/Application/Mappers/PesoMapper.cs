using Amandaba.Application.Dtos.Pesos;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class PesoMapper
    {
        public static PetPesoEntity ToEntity(this PesoRequestDto dto, decimal idPet)
        {
            return new PetPesoEntity
            {
                IdPet = idPet,
                Peso = dto.Peso,
                DataMedicao = dto.DataMedicao,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(this PesoRequestDto dto, PetPesoEntity entity)
        {
            entity.Peso = dto.Peso;
            entity.DataMedicao = dto.DataMedicao;
        }

        public static PesoResponseDto ToResponseDto(this PetPesoEntity entity)
        {
            return new PesoResponseDto
            {
                IdHistoricoPeso = entity.IdHistoricoPeso,
                IdPet = entity.IdPet,
                Peso = entity.Peso,
                DataMedicao = entity.DataMedicao,
                DataCadastro = entity.DataCadastro
            };
        }
    }
}