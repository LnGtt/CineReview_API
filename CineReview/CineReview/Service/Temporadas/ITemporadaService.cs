using CineReview.DTOs;

namespace CineReview.Services
{
    public interface ITemporadaService
    {
        Task<TemporadaRespostaDTO> CadastrarTemporadaAsync(CriarTemporadaDTO dto);
        Task AdicionarEpisodioAsync(CriarEpisodioDTO dto);
        Task<List<TemporadaRespostaDTO>> ListarPorSerieAsync(Guid serieId);
        Task AtualizarTemporadaAsync(Guid id, AtualizarTemporadaDTO dto);
        Task DeletarTemporadaAsync(Guid id);
    }
}