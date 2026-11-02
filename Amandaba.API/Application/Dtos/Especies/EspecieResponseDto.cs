namespace Amandaba.Application.Dtos.Especies
{
    public class EspecieResponseDto
    {
        public List<Amandaba.API.Application.Dtos.LinkDto> Links { get; set; } = new();

        public decimal IdEspecie { get; set; }

        public string Nome { get; set; } = string.Empty;
    }
}