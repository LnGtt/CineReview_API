using CineReview.DTOs;
using CineReview.DTOs.Externo;
using System.Globalization;
using System.Text.Json;

namespace CineReview.Service.Externo
{
    public class OmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["Omdb:Url"]!);
            _apiKey = configuration["Omdb:ApiKey"]!;
        }

        public async Task<CriarFilmeDTO> BuscarFilmePorTituloAsync(string titulo)
        {
            // Adicionamos Options para ignorar Maiúsculas/Minúsculas (Segurança extra)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.GetAsync($"?t={titulo}&apikey={_apiKey}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro HTTP ao contatar OMDb: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            // --- MUDANÇA AQUI: Vamos ver o que veio antes de converter ---
            // Tenta converter
            var dadosOmdb = JsonSerializer.Deserialize<OmdbFilmeResposta>(jsonString, options);

            // Se o Titulo for nulo, significa que o OMDb devolveu um erro (ex: "Invalid API Key" ou "Movie not found")
            if (dadosOmdb == null || string.IsNullOrEmpty(dadosOmdb.Titulo))
            {
                // AQUI ESTÁ O SEGREDO: Lançamos o JSON bruto para veres no Swagger o que aconteceu
                throw new Exception($"OMDb retornou erro: {jsonString}");
            }

            return new CriarFilmeDTO
            {
                Titulo = dadosOmdb.Titulo,
                Genero = dadosOmdb.Genero,
                Sinopse = dadosOmdb.Sinopse,
                ClassificacaoIndicativa = dadosOmdb.Classificacao,
                Duracao = ConverterDuracao(dadosOmdb.DuracaoTexto),
                DataLancamento = ConverterData(dadosOmdb.DataLancamentoTexto)
            };
        }

        // Métodos auxiliares para limpar os dados sujos da API externa
        private TimeSpan ConverterDuracao(string duracaoTexto)
        {
            // Pega apenas os números de "136 min"
            var apenasNumeros = new string(duracaoTexto.Where(char.IsDigit).ToArray());
            if (int.TryParse(apenasNumeros, out int minutos))
            {
                return TimeSpan.FromMinutes(minutos);
            }
            return TimeSpan.Zero;
        }

        private DateOnly ConverterData(string dataTexto)
        {
            // Tenta converter formatos americanos
            if (DateTime.TryParse(dataTexto, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
            {
                return DateOnly.FromDateTime(data);
            }
            return DateOnly.FromDateTime(DateTime.Now); // Fallback
        }
    }
}
