namespace Amandaba.Domain.Entities
{
    public class ConsultaEntity
    {
        public decimal IdConsulta { get; set; }

        public decimal IdPet { get; set; }

        public DateTime DataConsulta { get; set; }

        public string? Horario { get; set; }

        public string? Veterinario { get; set; }

        public string? Clinica { get; set; }

        public string? Motivo { get; set; }

        public string? Sintomas { get; set; }

        public decimal? Peso { get; set; }

        public string? Diagnostico { get; set; }

        public string? Tratamento { get; set; }

        public string? Observacao { get; set; }

        public bool Retorno { get; set; }

        public DateTime? DataRetorno { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public PetEntity Pet { get; set; } = null!;
    }
}