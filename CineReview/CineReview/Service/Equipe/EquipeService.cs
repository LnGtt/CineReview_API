using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly DataContext _context;

        public EquipeService(DataContext context)
        {
            _context = context;
        }

        public async Task<EquipeRespostaDTO> AdicionarAtorAsync(CriarAtorDTO dto)
        {
            if (dto.MidiaId == null && dto.TemporadaId == null)
                throw new Exception("É necessário informar uma Mídia ou uma Temporada.");

            var ator = new Ator(dto.NomeCompleto, dto.Funcoes, dto.Papel);

            await VincularMembroAsync(ator, dto.MidiaId, dto.TemporadaId);

            _context.Equipes.Add(ator);
            await _context.SaveChangesAsync();

            return ConverterParaDto(ator, "Ator");
        }

        public async Task<EquipeRespostaDTO> AdicionarTecnicoAsync(CriarTecnicoDTO dto)
        {
            if (dto.MidiaId == null && dto.TemporadaId == null)
                throw new Exception("É necessário informar uma Mídia ou uma Temporada.");

            var tecnico = new EquipeTecnica(dto.NomeCompleto, dto.Funcoes);

            await VincularMembroAsync(tecnico, dto.MidiaId, dto.TemporadaId);

            _context.Equipes.Add(tecnico);
            await _context.SaveChangesAsync();

            return ConverterParaDto(tecnico, "Tecnico");
        }

        private async Task VincularMembroAsync(Equipe equipe, Guid? midiaId, Guid? temporadaId)
        {
            if (midiaId.HasValue)
            {
                var midia = await _context.Midias.Include(m => m.Equipe).FirstOrDefaultAsync(m => m.Id == midiaId);
                if (midia == null) throw new Exception("Mídia não encontrada.");
                midia.AdicionarMembroEquipe(equipe);
            }
            else if (temporadaId.HasValue)
            {
                var temporada = await _context.Temporadas.Include(t => t.Equipe).FirstOrDefaultAsync(t => t.Id == temporadaId);
                if (temporada == null) throw new Exception("Temporada não encontrada.");
                temporada.AdicionarMembroEquipe(equipe);
            }
        }

        public async Task<List<EquipeRespostaDTO>> ListarPorMidiaAsync(Guid midiaId)
        {
            var midia = await _context.Midias
                .Include(m => m.Equipe)
                .FirstOrDefaultAsync(m => m.Id == midiaId);

            if (midia == null) return new List<EquipeRespostaDTO>();

            return midia.Equipe.Select(e => ConverterParaDto(e, e is Ator ? "Ator" : "Tecnico")).ToList();
        }

        public async Task<List<EquipeRespostaDTO>> ListarPorTemporadaAsync(Guid temporadaId)
        {
            var temporada = await _context.Temporadas
                .Include(t => t.Equipe)
                .FirstOrDefaultAsync(t => t.Id == temporadaId);

            if (temporada == null) return new List<EquipeRespostaDTO>();

            return temporada.Equipe.Select(e => ConverterParaDto(e, e is Ator ? "Ator" : "Tecnico")).ToList();
        }

        public async Task AtualizarAsync(Guid id, AtualizarEquipeDTO dto)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null) throw new Exception("Membro não encontrado.");

            equipe.NomeCompleto = dto.NomeCompleto;
            equipe.Funcoes.Clear();
            equipe.Funcoes.AddRange(dto.Funcoes);

            if (equipe is Ator ator && !string.IsNullOrEmpty(dto.Papel))
            {
                ator.Papel = dto.Papel;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null) throw new Exception("Membro não encontrado.");

            _context.Equipes.Remove(equipe);
            await _context.SaveChangesAsync();
        }

        private EquipeRespostaDTO ConverterParaDto(Equipe equipe, string tipo)
        {
            return new EquipeRespostaDTO
            {
                Id = equipe.Id,
                NomeCompleto = equipe.NomeCompleto,
                Funcoes = equipe.Funcoes,
                Tipo = tipo,
                Papel = (equipe as Ator)?.Papel
            };
        }
    }
}