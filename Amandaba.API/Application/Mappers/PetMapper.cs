using Amandaba.Application.Dtos.Pets;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class PetMapper
    {
        public static PetEntity ToEntity(this PetRequestDto dto, decimal idTutor)
        {
            return new PetEntity
            {
                IdTutor = idTutor,
                IdEspecie = dto.IdEspecie,
                Nome = dto.Nome,
                FotoUrl = dto.FotoUrl,
                Raca = dto.Raca,
                Sexo = dto.Sexo,
                DataNascimento = dto.DataNascimento,
                Cor = dto.Cor,
                Castrado = dto.Castrado,
                Microchip = dto.Microchip,
                DataCadastro = DateTime.Now,
                Status = "ATIVO"
            };
        }

        public static void UpdateEntity(this PetRequestDto dto, PetEntity entity)
        {
            entity.IdEspecie = dto.IdEspecie;
            entity.Nome = dto.Nome;
            entity.FotoUrl = dto.FotoUrl;
            entity.Raca = dto.Raca;
            entity.Sexo = dto.Sexo;
            entity.DataNascimento = dto.DataNascimento;
            entity.Cor = dto.Cor;
            entity.Castrado = dto.Castrado;
            entity.Microchip = dto.Microchip;
        }

        public static PetResponseDto ToResponseDto(this PetEntity entity)
        {
            return new PetResponseDto
            {
                IdPet = entity.IdPet,
                IdTutor = entity.IdTutor,
                IdEspecie = entity.IdEspecie,
                Especie = entity.Especie?.Nome ?? string.Empty,
                Nome = entity.Nome,
                FotoUrl = entity.FotoUrl,
                Raca = entity.Raca,
                Sexo = entity.Sexo,
                DataNascimento = entity.DataNascimento,
                Cor = entity.Cor,
                Castrado = entity.Castrado,
                Microchip = entity.Microchip,
                DataCadastro = entity.DataCadastro,
                Status = entity.Status
            };
        }
    }
}