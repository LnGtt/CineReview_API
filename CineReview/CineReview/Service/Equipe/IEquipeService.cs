using CineReview.DTOs;

namespace CineReview.Services
{
    public interface IEquipeService
    {
        Task<EquipeRespostaDTO> AdicionarAtorAsync(CriarAtorDTO dto);
        Task<EquipeRespostaDTO> AdicionarTecnicoAsync(CriarTecnicoDTO dto);
        Task<List<EquipeRespostaDTO>> ListarPorMidiaAsync(Guid midiaId);
        Task<List<EquipeRespostaDTO>> ListarPorTemporadaAsync(Guid temporadaId);
        Task AtualizarAsync(Guid id, AtualizarEquipeDTO dto);
        Task DeletarAsync(Guid id);
    }
}