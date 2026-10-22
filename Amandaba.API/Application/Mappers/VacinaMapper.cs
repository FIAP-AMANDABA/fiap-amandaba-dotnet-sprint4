using Amandaba.Application.Dtos.Vacinas;
using Amandaba.Domain.Entities;

namespace Amandaba.Application.Mappers
{
    public static class VacinaMapper
    {
        public static VacinaCatalogoResponseDto ToCatalogoResponseDto(
            this VacinaEntity entity)
        {
            return new VacinaCatalogoResponseDto
            {
                IdVacina = entity.IdVacina,
                Nome = entity.Nome,
                Tipo = entity.Tipo,
                Fabricante = entity.Fabricante,
                Descricao = entity.Descricao
            };
        }

        public static PetVacinaEntity ToEntity(
            this VacinaAplicacaoRequestDto dto,
            decimal idPet)
        {
            return new PetVacinaEntity
            {
                IdPet = idPet,
                IdVacina = dto.IdVacina,
                DataAplicacao = dto.DataAplicacao,
                NumeroDose = dto.NumeroDose,
                NumeroLote = dto.NumeroLote,
                ProximaDose = dto.ProximaDose,
                Clinica = dto.Clinica,
                Observacao = dto.Observacao,
                Comprovante = dto.Comprovante,
                DataCadastro = DateTime.Now
            };
        }

        public static void UpdateEntity(
            this VacinaAplicacaoRequestDto dto,
            PetVacinaEntity entity)
        {
            entity.IdVacina = dto.IdVacina;
            entity.DataAplicacao = dto.DataAplicacao;
            entity.NumeroDose = dto.NumeroDose;
            entity.NumeroLote = dto.NumeroLote;
            entity.ProximaDose = dto.ProximaDose;
            entity.Clinica = dto.Clinica;
            entity.Observacao = dto.Observacao;
            entity.Comprovante = dto.Comprovante;
        }

        public static VacinaAplicacaoResponseDto ToAplicacaoResponseDto(
            this PetVacinaEntity entity)
        {
            return new VacinaAplicacaoResponseDto
            {
                IdAplicacaoVacina = entity.IdAplicacaoVacina,
                IdPet = entity.IdPet,
                Vacina = entity.Vacina.ToCatalogoResponseDto(),
                DataAplicacao = entity.DataAplicacao,
                NumeroDose = entity.NumeroDose,
                NumeroLote = entity.NumeroLote,
                ProximaDose = entity.ProximaDose,
                Situacao = CalcularSituacao(entity.ProximaDose),
                Clinica = entity.Clinica,
                Observacao = entity.Observacao,
                Comprovante = entity.Comprovante,
                DataCadastro = entity.DataCadastro
            };
        }

        private static string? CalcularSituacao(DateTime? proximaDose)
        {
            if (!proximaDose.HasValue)
                return null;

            return proximaDose.Value.Date >= DateTime.Today
                ? "PROXIMA"
                : "ATRASADA";
        }
    }
}