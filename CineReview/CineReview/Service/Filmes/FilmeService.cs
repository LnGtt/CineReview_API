using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Services
{
    public class FilmeService : IFilmeService
    {
        private readonly DataContext _context;

        public FilmeService(DataContext context)
        {
            _context = context;
        }

        public async Task<FilmeRespostaDTO> CadastrarAsync(CriarFilmeDTO dto)
        {
            if (await _context.Midias.AnyAsync(m => m.Titulo == dto.Titulo))
                throw new Exception("Já existe um filme com este título.");

            var novoFilme = new CineReview.Models.Filme(
                dto.Titulo, dto.Genero, dto.Sinopse, dto.Duracao,
                dto.ClassificacaoIndicativa, dto.DataLancamento
            );

            _context.Midias.Add(novoFilme);
            await _context.SaveChangesAsync();

            return new FilmeRespostaDTO
            {
                Id = novoFilme.Id,
                Titulo = novoFilme.Titulo,
                Genero = novoFilme.Genero,
                NotaMediaGeral = novoFilme.NotaMediaGeral
            };
        }

        public async Task<List<FilmeRespostaDTO>> ListarTodosAsync()
        {
            var filmes = await _context.Midias.OfType<CineReview.Models.Filme>().ToListAsync();

            return filmes.Select(f => new FilmeRespostaDTO
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Genero = f.Genero,
                NotaMediaGeral = f.NotaMediaGeral
            }).ToList();
        }

        public async Task<FilmeRespostaDTO> BuscarPorIdAsync(Guid id)
        {
            var filme = await _context.Midias.OfType<CineReview.Models.Filme>()
                                     .FirstOrDefaultAsync(f => f.Id == id);

            if (filme == null) throw new Exception("Filme não encontrado.");

            return new FilmeRespostaDTO
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Genero = filme.Genero,
                NotaMediaGeral = filme.NotaMediaGeral
            };
        }

        public async Task AtualizarAsync(Guid id, CriarFilmeDTO dto)
        {
            var filme = await _context.Midias.OfType<CineReview.Models.Filme>()
                                     .FirstOrDefaultAsync(f => f.Id == id);

            if (filme == null) throw new Exception("Filme não encontrado.");

            filme.Titulo = dto.Titulo;
            filme.Genero = dto.Genero;
            filme.Sinopse = dto.Sinopse;
            filme.Duracao = dto.Duracao;
            filme.ClassificacaoIndicativa = dto.ClassificacaoIndicativa;
            filme.DataLancamento = dto.DataLancamento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var mensagemErro = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Erro ao atualizar no banco: {mensagemErro}");
            }
        }

        public async Task DeletarAsync(Guid id)
        {
            var filme = await _context.Midias.FindAsync(id);
            if (filme == null) throw new Exception("Filme não encontrado.");

            _context.Midias.Remove(filme);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FilmeRespostaDTO>> ListarRankingAsync()
        {
            var filmes = await _context.Midias.OfType<CineReview.Models.Filme>()
                                     .Include(f => f.Avaliacoes) 
                                     .ToListAsync();

            return filmes
                .OrderByDescending(f => f.NotaMediaGeral) 
                .Select(f => new FilmeRespostaDTO
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Genero = f.Genero,
                    NotaMediaGeral = f.NotaMediaGeral
                })
                .ToList();
        }

        public async Task<List<FilmeRespostaDTO>> FiltrarPorGeneroAsync(string genero)
        {
            
            var filmes = await _context.Midias.OfType<CineReview.Models.Filme>()
                                     .Where(f => f.Genero.Contains(genero)) 
                                     .ToListAsync();

            return filmes.Select(f => new FilmeRespostaDTO
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Genero = f.Genero,
                NotaMediaGeral = f.NotaMediaGeral
            }).ToList();
        }
    }
}