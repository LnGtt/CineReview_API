using CineReview.Data;
using CineReview.DTOs;
using CineReview.Models;
using CineReview.Service.Avaliacao;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineReview.Services
{

    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly DataContext _context;

        public AvaliacaoService(DataContext context)
        {
            _context = context;
        }

        public async Task<AvaliacaoRespostaDTO> AvaliarAsync(CriarAvaliacaoDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) throw new Exception("Usuário não encontrado.");

            bool existeMidia = await _context.Midias.AnyAsync(m => m.Id == dto.AvaliadoId);
            bool existeTemporada = await _context.Temporadas.AnyAsync(t => t.Id == dto.AvaliadoId);

            if (!existeMidia && !existeTemporada)
                throw new Exception("Obra (Filme/Série/Temporada) não encontrada.");

            bool jaAvaliou = await _context.Avaliacoes.AnyAsync(a =>
                a.UsuarioId == dto.UsuarioId &&
                a.AvaliadoId == dto.AvaliadoId);

            if (jaAvaliou) throw new Exception("Você já avaliou esta obra.");

            var novaAvaliacao = new CineReview.Models.Avaliacao(
                usuario, dto.AvaliadoId,
                dto.NotaTrama, dto.NotaRitmo, dto.NotaDevPersonagens, dto.NotaConstrucaoMundo, dto.NotaTematica,
                dto.NotaAtuacao, dto.NotaEdicao, dto.NotaDirecao,
                dto.NotaArte, dto.NotaCinematografia, dto.NotaCenarios, dto.NotaFigurinos, dto.NotaEfeitosVisuais, dto.NotaQualidadeImagem,
                dto.NotaScore, dto.NotaEfeitosSonoros
            );

            _context.Avaliacoes.Add(novaAvaliacao);
            await _context.SaveChangesAsync();

            return ConverterParaDto(novaAvaliacao);
        }

        public async Task<List<AvaliacaoRespostaDTO>> ListarTodasAsync()
        {
            var lista = await _context.Avaliacoes
                .Include(a => a.Usuario)
                .ToListAsync();

            return lista.Select(a => ConverterParaDto(a)).ToList();
        }

        public async Task<List<AvaliacaoRespostaDTO>> ListarPorMidiaAsync(Guid midiaId)
        {
            var lista = await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Where(a => a.AvaliadoId == midiaId)
                .ToListAsync();

            return lista.Select(a => ConverterParaDto(a)).ToList();
        }

        public async Task DeletarAsync(Guid id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null) throw new Exception("Avaliação não encontrada.");
            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }

        private AvaliacaoRespostaDTO ConverterParaDto(CineReview.Models.Avaliacao a)
        {
            return new AvaliacaoRespostaDTO
            {
                Id = a.Id,
                NomeUsuario = a.Usuario?.NomeUsuario ?? "Anônimo",
                AvaliadoId = a.AvaliadoId,
                MediaGeral = a.GetMediaGeral(),
                DataAvaliacao = a.DataAvaliacao
            };
        }
    }
}