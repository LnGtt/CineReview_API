using System.ComponentModel.DataAnnotations;

namespace CineReview.DTOs
{
    public class CriarTemporadaDTO
    {
        [Required] public Guid SerieId { get; set; }
        [Required] public int NumeroTemporada { get; set; }
        [Required] public string Titulo { get; set; }
        [Required] public string Sinopse { get; set; }
        [Required] public string ClassificacaoIndicativa { get; set; }
        [Required] public DateOnly DataLancamento { get; set; }
    }

    public class AtualizarTemporadaDTO
    {
        [Required] public string Titulo { get; set; }
        [Required] public string Sinopse { get; set; }
        [Required] public string ClassificacaoIndicativa { get; set; }
        [Required] public DateOnly DataLancamento { get; set; }
    }

    public class TemporadaRespostaDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public int Numero { get; set; }
        public double NotaMedia { get; set; }
        public int QtdEpisodios { get; set; }
    }
}