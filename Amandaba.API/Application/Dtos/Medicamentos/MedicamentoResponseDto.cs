namespace Amandaba.Application.Dtos.Medicamentos
{
    public class MedicamentoResponseDto
    {
        public decimal IdRegistroMedicamento { get; set; }

        public decimal IdPet { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Motivo { get; set; }

        public decimal? Dosagem { get; set; }

        public string? Unidade { get; set; }

        public string? Quantidade { get; set; }

        public string? Frequencia { get; set; }

        public string? Administracao { get; set; }

        public string? Horario { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataTermino { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Prescricao { get; set; }

        public string? Observacao { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}