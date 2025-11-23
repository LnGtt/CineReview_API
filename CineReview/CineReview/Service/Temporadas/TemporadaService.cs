using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Services
{
    public class TemporadaService : ITemporadaService
    {
        private readonly DataContext _context;

        public TemporadaService(DataContext context)
        {
            _context = context;
        }

        public async Task<TemporadaRespostaDTO> CadastrarTemporadaAsync(CriarTemporadaDTO dto)
        {
            var serie = await _context.Midias.OfType<CineReview.Models.Serie>()
                        .Include(s => s.Temporadas)
                        .FirstOrDefaultAsync(s => s.Id == dto.SerieId);

            if (serie == null) throw new Exception("Série não encontrada.");

            if (serie.Temporadas.Any(t => t.NumeroTemporada == dto.NumeroTemporada))
                throw new Exception($"A temporada {dto.NumeroTemporada} já existe nesta série.");

            var novaTemporada = new CineReview.Models.Temporada(
                dto.NumeroTemporada, dto.Titulo, dto.Sinopse,
                dto.ClassificacaoIndicativa, dto.DataLancamento
            );

            serie.AdicionarTemporada(novaTemporada);
            _context.Temporadas.Add(novaTemporada);
            await _context.SaveChangesAsync();

            return new TemporadaRespostaDTO
            {
                Id = novaTemporada.Id,
                Titulo = novaTemporada.Titulo,
                Numero = novaTemporada.NumeroTemporada,
                NotaMedia = 0,
                QtdEpisodios = 0
            };
        }

        public async Task AdicionarEpisodioAsync(CriarEpisodioDTO dto)
        {
            var temporada = await _context.Temporadas
                            .Include(t => t.Episodios)
                            .FirstOrDefaultAsync(t => t.Id == dto.TemporadaId);

            if (temporada == null) throw new Exception("Temporada não encontrada.");

            if (temporada.Episodios.Any(e => e.NumeroEpisodio == dto.NumeroEpisodio))
                throw new Exception($"Episódio {dto.NumeroEpisodio} já existe.");

            var novoEpisodio = new CineReview.Models.Episodio(
                dto.NumeroEpisodio, dto.Titulo, dto.Sinopse, dto.Duracao
            );

            temporada.AdicionarEpisodio(novoEpisodio);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TemporadaRespostaDTO>> ListarPorSerieAsync(Guid serieId)
        {
            var lista = await _context.Temporadas
                .Include(t => t.Avaliacoes)
                .Include(t => t.Episodios)
                .ToListAsync();

            return lista
                .Where(t => _context.Midias.OfType<CineReview.Models.Serie>()
                            .Any(s => s.Id == serieId && s.Temporadas.Contains(t)))
                .Select(t => new TemporadaRespostaDTO
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Numero = t.NumeroTemporada,
                    NotaMedia = t.NotaMediaGeral,
                    QtdEpisodios = t.Episodios.Count
                }).ToList();
        }

        public async Task AtualizarTemporadaAsync(Guid id, AtualizarTemporadaDTO dto)
        {
            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null) throw new Exception("Temporada não encontrada.");

            temporada.Titulo = dto.Titulo;
            temporada.Sinopse = dto.Sinopse;
            temporada.ClassificacaoIndicativa = dto.ClassificacaoIndicativa;
            temporada.DataLancamento = dto.DataLancamento;

            await _context.SaveChangesAsync();
        }

        public async Task DeletarTemporadaAsync(Guid id)
        {
            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null) throw new Exception("Temporada não encontrada.");

            _context.Temporadas.Remove(temporada);
            await _context.SaveChangesAsync();
        }
    }
}