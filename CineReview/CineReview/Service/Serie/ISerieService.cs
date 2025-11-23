using CineReview.DTOs;

namespace CineReview.Services
{
    public interface ISerieService
    {
        Task<SerieRespostaDTO> CadastrarAsync(CriarSerieDTO dto);
        Task<List<SerieRespostaDTO>> ListarTodasAsync();
        Task<SerieRespostaDTO> BuscarPorIdAsync(Guid id);
        Task AtualizarAsync(Guid id, CriarSerieDTO dto);
        Task DeletarAsync(Guid id);
    }
}