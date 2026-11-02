namespace Amandaba.Application.Dtos.Doencas
{
    public class DoencaResponseDto
    {
        public List<Amandaba.API.Application.Dtos.LinkDto> Links { get; set; } = new();

        public decimal IdRegistroDoenca { get; set; }

        public decimal IdPet { get; set; }

        public string Nome { get; set; } = string.Empty;

        public DateTime? DataDiagnostico { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Tratamento { get; set; }

        public string? Observacao { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}