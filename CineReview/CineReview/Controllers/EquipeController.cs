using CineReview.DTOs;
using CineReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _service;

        public EquipeController(IEquipeService service)
        {
            _service = service;
        }

        [HttpPost("ator")]
        public async Task<IActionResult> AdicionarAtor([FromBody] CriarAtorDTO dto)
        {
            try
            {
                var resultado = await _service.AdicionarAtorAsync(dto);
                return Ok(resultado);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("tecnico")]
        public async Task<IActionResult> AdicionarTecnico([FromBody] CriarTecnicoDTO dto)
        {
            try
            {
                var resultado = await _service.AdicionarTecnicoAsync(dto);
                return Ok(resultado);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("midia/{midiaId}")]
        public async Task<IActionResult> ListarPorMidia(Guid midiaId)
        {
            return Ok(await _service.ListarPorMidiaAsync(midiaId));
        }

        [HttpGet("temporada/{temporadaId}")]
        public async Task<IActionResult> ListarPorTemporada(Guid temporadaId)
        {
            return Ok(await _service.ListarPorTemporadaAsync(temporadaId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEquipeDTO dto)
        {
            try
            {
                await _service.AtualizarAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _service.DeletarAsync(id);
                return NoContent();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}