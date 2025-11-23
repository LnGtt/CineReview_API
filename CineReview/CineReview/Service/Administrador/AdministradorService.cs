using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using CineReview.Service.Administrador;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly DataContext _context;

        public AdministradorService(DataContext context)
        {
            _context = context;
        }

        public async Task<AdministradorRespostaDTO> CadastrarAsync(CriarAdministradorDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Este email já está cadastrado.");

            string senhaHash = dto.Senha.GetHashCode().ToString();

            var novoAdmin = new Administrador(dto.NomeUsuario, dto.Email, senhaHash);

            _context.Usuarios.Add(novoAdmin);
            await _context.SaveChangesAsync();

            return ConverterParaDto(novoAdmin);
        }

        public async Task<AdministradorRespostaDTO> LoginAsync(LoginUsuarioDTO dto)
        {
            var admin = await _context.Usuarios.OfType<Administrador>()
                                     .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (admin == null)
                throw new Exception("Administrador não encontrado ou credenciais inválidas.");

            if (!admin.ValidarSenha(dto.Senha))
                throw new Exception("Credenciais inválidas.");

            return ConverterParaDto(admin);
        }

        public async Task<List<AdministradorRespostaDTO>> ListarTodosAsync()
        {
            var admins = await _context.Usuarios.OfType<Administrador>().ToListAsync();
            return admins.Select(a => ConverterParaDto(a)).ToList();
        }

        public async Task<AdministradorRespostaDTO> BuscarPorIdAsync(Guid id)
        {
            var admin = await _context.Usuarios.OfType<Administrador>()
                                     .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null) throw new Exception("Administrador não encontrado.");

            return ConverterParaDto(admin);
        }

        public async Task AtualizarAsync(Guid id, AtualizarAdministradorDTO dto)
        {
            var admin = await _context.Usuarios.OfType<Administrador>()
                                     .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null) throw new Exception("Administrador não encontrado.");

            admin.NomeUsuario = dto.NomeUsuario;
            admin.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var admin = await _context.Usuarios.OfType<Administrador>()
                                     .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null) throw new Exception("Administrador não encontrado.");

            _context.Usuarios.Remove(admin);
            await _context.SaveChangesAsync();
        }

        private AdministradorRespostaDTO ConverterParaDto(Administrador admin)
        {
            return new AdministradorRespostaDTO
            {
                Id = admin.Id,
                NomeUsuario = admin.NomeUsuario,
                Email = admin.Email,
                Tipo = "Administrador"
            };
        }
    }
}