namespace Amandaba.Domain.Entities
{
    public class VacinaEntity
    {
        public decimal IdVacina { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Tipo { get; set; }

        public string? Fabricante { get; set; }

        public string? Descricao { get; set; }

        public ICollection<PetVacinaEntity> Aplicacoes { get; set; } = new List<PetVacinaEntity>();
    }
}