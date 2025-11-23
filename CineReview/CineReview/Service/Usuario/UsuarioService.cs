using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using CineReview.Service.Usuario;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Services
{

    public class UsuarioService : IUsuarioService
    {
        private readonly DataContext _context;

        public UsuarioService(DataContext context)
        {
            _context = context;
        }

        public async Task<UsuarioRespostaDTO> CadastrarAsync(CriarUsuarioDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Este email já está cadastrado.");

            string senhaHash = dto.Senha.GetHashCode().ToString();
            var novoUsuario = new Usuario(dto.NomeUsuario, dto.Email, senhaHash);

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return new UsuarioRespostaDTO
            {
                Id = novoUsuario.Id,
                NomeUsuario = novoUsuario.NomeUsuario,
                Email = novoUsuario.Email
            };
        }

        public async Task<UsuarioRespostaDTO> LoginAsync(LoginUsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null) throw new Exception("Usuário ou senha inválidos.");

            if (!usuario.ValidarSenha(dto.Senha))
                throw new Exception("Usuário ou senha inválidos.");

            string tipoUsuario = usuario is Administrador ? "Administrador" : "Comum";

            return new UsuarioRespostaDTO
            {
                Id = usuario.Id,
                NomeUsuario = usuario.NomeUsuario,
                Email = usuario.Email,
                Tipo = tipoUsuario
            };
        }

        public async Task<UsuarioRespostaDTO> BuscarPorIdAsync(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) throw new Exception("Usuário não encontrado.");

            return ConverterParaDto(usuario);
        }

        public async Task<List<UsuarioRespostaDTO>> ListarTodosAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            return usuarios.Select(u => new UsuarioRespostaDTO
            {
                Id = u.Id,
                NomeUsuario = u.NomeUsuario,
                Email = u.Email,
                Tipo = u is Administrador ? "Administrador" : "Comum"
            }).ToList();
        }

        public async Task AtualizarAsync(Guid id, AtualizarUsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) throw new Exception("Usuário não encontrado.");

            usuario.NomeUsuario = dto.NomeUsuario;
            usuario.Email = dto.Email;
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) throw new Exception("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }


        private UsuarioRespostaDTO ConverterParaDto(Usuario u)
        {
            return new UsuarioRespostaDTO
            {
                Id = u.Id,
                NomeUsuario = u.NomeUsuario,
                Email = u.Email,
                Tipo = u is Administrador ? "Administrador" : "Comum"
            };
        }
    }
}