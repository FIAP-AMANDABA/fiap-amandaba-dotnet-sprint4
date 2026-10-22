namespace Amandaba.Application.Dtos.Vacinas
{
    public class VacinaAplicacaoResponseDto
    {
        public decimal IdAplicacaoVacina { get; set; }

        public decimal IdPet { get; set; }

        public VacinaCatalogoResponseDto Vacina { get; set; } = new();

        public DateTime DataAplicacao { get; set; }

        public decimal? NumeroDose { get; set; }

        public string? NumeroLote { get; set; }

        public DateTime? ProximaDose { get; set; }

        public string? Situacao { get; set; }

        public string? Clinica { get; set; }

        public string? Observacao { get; set; }

        public string? Comprovante { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}