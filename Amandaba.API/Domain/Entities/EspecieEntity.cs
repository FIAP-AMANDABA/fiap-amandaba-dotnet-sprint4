namespace Amandaba.Domain.Entities
{
    public class EspecieEntity
    {
        public decimal IdEspecie { get; set; }

        public string Nome { get; set; } = string.Empty;

        public ICollection<PetEntity> Pets { get; set; } = new List<PetEntity>();
    }
}