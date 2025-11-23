using System.Text.Json.Serialization;

namespace CineReview.DTOs.Externo
{
    public class OmdbFilmeResposta
    {
        [JsonPropertyName("Title")]
        public string Titulo { get; set; }

        [JsonPropertyName("Genre")]
        public string Genero { get; set; }

        [JsonPropertyName("Plot")]
        public string Sinopse { get; set; }

        [JsonPropertyName("Runtime")]
        public string DuracaoTexto { get; set; } // Vem como "120 min"

        [JsonPropertyName("Released")]
        public string DataLancamentoTexto { get; set; } // Vem como "17 Dec 2021"

        [JsonPropertyName("Rated")]
        public string Classificacao { get; set; }
    }
}
