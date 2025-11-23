using CineReview.DTOs;
using CineReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemporadaController : ControllerBase
    {
        private readonly ITemporadaService _service;

        public TemporadaController(ITemporadaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarTemporada([FromBody] CriarTemporadaDTO dto)
        {
            try
            {
                var resultado = await _service.CadastrarTemporadaAsync(dto);
                return CreatedAtAction(nameof(ListarPorSerie), new { serieId = dto.SerieId }, resultado);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("episodio")]
        public async Task<IActionResult> AdicionarEpisodio([FromBody] CriarEpisodioDTO dto)
        {
            try
            {
                await _service.AdicionarEpisodioAsync(dto);
                return Ok("Episódio adicionado com sucesso.");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("serie/{serieId}")]
        public async Task<IActionResult> ListarPorSerie(Guid serieId)
        {
            return Ok(await _service.ListarPorSerieAsync(serieId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarTemporadaDTO dto)
        {
            try
            {
                await _service.AtualizarTemporadaAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _service.DeletarTemporadaAsync(id);
                return NoContent();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}