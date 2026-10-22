using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Amandaba.API.Application.UseCases
{
    public class GeminiUseCase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public GeminiUseCase(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            _apiUrl = configuration["Gemini:Url"];
        }

        public async Task<string> GerarPlanoDeCuidadosAsync(
            string nomePet,
            string especie,
            string medicamentosEmUso)
        {
            var urlComChave = $"{_apiUrl}?key={_apiKey}";

            var prompt =
                $"Aja como um veterinário especialista. O pet {nomePet} " +
                $"(Espécie: {especie}) está atualmente com os seguintes " +
                $"medicamentos em uso: {medicamentosEmUso}. " +
                $"Escreva um breve plano de cuidados diários para o tutor, " +
                $"com 3 dicas de manejo, em tom amigável.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(urlComChave, content);

            if (!response.IsSuccessStatusCode)
            {
                var erroDetalhado = await response.Content.ReadAsStringAsync();
                return $"Erro {response.StatusCode} do Gemini: {erroDetalhado}";
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseJson);

            var textoGerado = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return textoGerado ?? string.Empty;
        }
    }
}

