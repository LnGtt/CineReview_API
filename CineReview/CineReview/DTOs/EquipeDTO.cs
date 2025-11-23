using System.ComponentModel.DataAnnotations;

namespace CineReview.DTOs
{
    public class CriarMembroEquipeDTO
    {
        public Guid? MidiaId { get; set; }
        public Guid? TemporadaId { get; set; }
        [Required] public string NomeCompleto { get; set; }
        [Required] public List<string> Funcoes { get; set; }
    }

    public class CriarAtorDTO : CriarMembroEquipeDTO
    {
        [Required] public string Papel { get; set; }
    }

    public class CriarTecnicoDTO : CriarMembroEquipeDTO
    {
    }

    public class EquipeRespostaDTO
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; }
        public string NomeCompleto { get; set; }
        public List<string> Funcoes { get; set; }
        public string? Papel { get; set; }
    }

    public class AtualizarEquipeDTO
    {
        public string NomeCompleto { get; set; }
        public List<string> Funcoes { get; set; }
        public string? Papel { get; set; }
    }
}