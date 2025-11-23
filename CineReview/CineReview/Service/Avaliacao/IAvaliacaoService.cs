using CineReview.DTOs;

namespace CineReview.Service.Avaliacao
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoRespostaDTO> AvaliarAsync(CriarAvaliacaoDTO dto);
        Task<List<AvaliacaoRespostaDTO>> ListarPorMidiaAsync(Guid midiaId);
        Task<List<AvaliacaoRespostaDTO>> ListarTodasAsync();
        Task DeletarAsync(Guid id);
    }
}
