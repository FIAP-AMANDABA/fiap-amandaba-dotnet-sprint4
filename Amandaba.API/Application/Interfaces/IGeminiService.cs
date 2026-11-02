namespace Amandaba.API.Application.Interfaces;

// Contrato do servico externo: a Application nao depende de HttpClient/Google Gemini.
public interface IGeminiService
{
    Task<string> GerarPlanoDeCuidadosAsync(string nomePet, string especie, string contexto);
}
