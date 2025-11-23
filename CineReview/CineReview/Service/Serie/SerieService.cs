using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Services
{
    public class SerieService : ISerieService
    {
        private readonly DataContext _context;

        public SerieService(DataContext context)
        {
            _context = context;
        }

        public async Task<SerieRespostaDTO> CadastrarAsync(CriarSerieDTO dto)
        {
            if (await _context.Midias.AnyAsync(m => m.Titulo == dto.Titulo))
                throw new Exception("Série já cadastrada.");

            var novaSerie = new CineReview.Models.Serie(
                dto.Titulo, dto.Genero, dto.Sinopse,
                dto.DuracaoMediaEpisodio,
                dto.ClassificacaoIndicativa, dto.DataLancamento
            );

            _context.Midias.Add(novaSerie);
            await _context.SaveChangesAsync();

            return new SerieRespostaDTO
            {
                Id = novaSerie.Id,
                Titulo = novaSerie.Titulo,
                Genero = novaSerie.Genero,
                NotaMediaGeral = 0,
                QtdTemporadas = 0
            };
        }

        public async Task<List<SerieRespostaDTO>> ListarTodasAsync()
        {
            var series = await _context.Midias.OfType<CineReview.Models.Serie>()
                .Include(s => s.Temporadas)
                .ThenInclude(t => t.Avaliacoes)
                .ToListAsync();

            return series.Select(s => new SerieRespostaDTO
            {
                Id = s.Id,
                Titulo = s.Titulo,
                Genero = s.Genero,
                NotaMediaGeral = s.NotaMediaGeral,
                QtdTemporadas = s.Temporadas.Count
            }).ToList();
        }

        public async Task<SerieRespostaDTO> BuscarPorIdAsync(Guid id)
        {
            var serie = await _context.Midias.OfType<CineReview.Models.Serie>()
                .Include(s => s.Temporadas)
                .ThenInclude(t => t.Avaliacoes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (serie == null) throw new Exception("Série não encontrada");
            
            return new SerieRespostaDTO
            {
                Id = serie.Id,
                Titulo = serie.Titulo,
                Genero = serie.Genero,
                NotaMediaGeral = serie.NotaMediaGeral,
                QtdTemporadas = serie.Temporadas.Count
            };
        }

        public async Task AtualizarAsync(Guid id, CriarSerieDTO dto)
        {
            var serie = await _context.Midias.OfType<CineReview.Models.Serie>()
                                    .FirstOrDefaultAsync(s => s.Id == id);

            if (serie == null) throw new Exception("Série não encontrada.");

            serie.Titulo = dto.Titulo;
            serie.Genero = dto.Genero;
            serie.Sinopse = dto.Sinopse;
            serie.ClassificacaoIndicativa = dto.ClassificacaoIndicativa;
            serie.DataLancamento = dto.DataLancamento;

            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var serie = await _context.Midias.OfType<CineReview.Models.Serie>()
                                    .FirstOrDefaultAsync(s => s.Id == id);

            if (serie == null) throw new Exception("Série não encontrada.");

            _context.Midias.Remove(serie);
            await _context.SaveChangesAsync();
        }
    }
}