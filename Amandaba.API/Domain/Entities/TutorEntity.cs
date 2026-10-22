namespace Amandaba.Domain.Entities
{
    public class TutorEntity
    {
        public decimal IdTutor { get; set; }

        public decimal IdUsuario { get; set; }

        public UsuarioEntity Usuario { get; set; } = null!;

        public ICollection<PetEntity> Pets { get; set; } = new List<PetEntity>();
    }
}