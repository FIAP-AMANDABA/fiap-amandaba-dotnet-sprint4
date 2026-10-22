namespace Amandaba.Domain.Entities
{
    public class UsuarioEntity
    {
        public decimal IdUsuario { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public DateTime? DataNascimento { get; set; }

        public string Email { get; set; } = string.Empty;

        public string? Telefone { get; set; }

        public string SenhaHash { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public string Status { get; set; } = string.Empty;

        public TutorEntity? Tutor { get; set; }
    }
}