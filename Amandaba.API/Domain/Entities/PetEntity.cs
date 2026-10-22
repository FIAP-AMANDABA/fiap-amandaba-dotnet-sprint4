namespace Amandaba.Domain.Entities
{
    public class PetEntity
    {
        public decimal IdPet { get; set; }

        public decimal IdTutor { get; set; }

        public decimal IdEspecie { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        public string? Raca { get; set; }

        public string? Sexo { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string? Cor { get; set; }

        public bool Castrado { get; set; }

        public string? Microchip { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Status { get; set; } = string.Empty;

        public TutorEntity Tutor { get; set; } = null!;

        public EspecieEntity Especie { get; set; } = null!;

        public ICollection<PetPesoEntity> Pesos { get; set; } = new List<PetPesoEntity>();

        public ICollection<PetVacinaEntity> Vacinas { get; set; } = new List<PetVacinaEntity>();

        public ICollection<PetDoencaEntity> Doencas { get; set; } = new List<PetDoencaEntity>();

        public ICollection<PetAlergiaEntity> Alergias { get; set; } = new List<PetAlergiaEntity>();

        public ICollection<PetMedicamentoEntity> Medicamentos { get; set; } = new List<PetMedicamentoEntity>();

        public ICollection<ConsultaEntity> Consultas { get; set; } = new List<ConsultaEntity>();

        public ICollection<ExameEntity> Exames { get; set; } = new List<ExameEntity>();
    }
}