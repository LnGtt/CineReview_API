using CineReview.DTOs;

namespace CineReview.Service.Administrador
{
    public interface IAdministradorService
    {
        Task<AdministradorRespostaDTO> CadastrarAsync(CriarAdministradorDTO dto);
        Task<AdministradorRespostaDTO> LoginAsync(LoginUsuarioDTO dto);
        Task<List<AdministradorRespostaDTO>> ListarTodosAsync();
        Task<AdministradorRespostaDTO> BuscarPorIdAsync(Guid id);
        Task AtualizarAsync(Guid id, AtualizarAdministradorDTO dto);
        Task DeletarAsync(Guid id);
    }
}
