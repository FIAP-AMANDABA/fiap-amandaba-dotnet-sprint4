namespace Amandaba.Application.Dtos.Exames
{
    public class ExameResponseDto
    {
        public decimal IdExame { get; set; }

        public decimal IdPet { get; set; }

        public string Nome { get; set; } = string.Empty;

        public DateTime? DataSolicitacao { get; set; }

        public DateTime? DataRealizacao { get; set; }

        public string? Veterinario { get; set; }

        public string? Clinica { get; set; }

        public string? Motivo { get; set; }

        public string? Resultado { get; set; }

        public string? Observacao { get; set; }

        public string? Arquivo { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }
    }
}