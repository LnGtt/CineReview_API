using CineReview.DTOs;

public interface IFilmeService
{
    Task<FilmeRespostaDTO> CadastrarAsync(CriarFilmeDTO dto);
    Task<List<FilmeRespostaDTO>> ListarTodosAsync();
    Task<FilmeRespostaDTO> BuscarPorIdAsync(Guid id);
    Task AtualizarAsync(Guid id, CriarFilmeDTO dto);
    Task DeletarAsync(Guid id);
}