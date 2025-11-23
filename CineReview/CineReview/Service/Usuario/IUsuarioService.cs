using CineReview.DTOs;
using CineReview.Models;

namespace CineReview.Service.Usuario
{
    public interface IUsuarioService
    {
        Task<UsuarioRespostaDTO> CadastrarAsync(CriarUsuarioDTO dto);
        Task<UsuarioRespostaDTO> LoginAsync(LoginUsuarioDTO dto);
        Task<List<UsuarioRespostaDTO>> ListarTodosAsync();
        Task<UsuarioRespostaDTO> BuscarPorIdAsync(Guid id);
        Task AtualizarAsync(Guid id, AtualizarUsuarioDTO dto);
        Task DeletarAsync(Guid id);
    }
}
