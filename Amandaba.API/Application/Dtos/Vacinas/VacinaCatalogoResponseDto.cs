namespace Amandaba.Application.Dtos.Vacinas
{
    public class VacinaCatalogoResponseDto
    {
        public decimal IdVacina { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Tipo { get; set; }

        public string? Fabricante { get; set; }

        public string? Descricao { get; set; }
    }
}