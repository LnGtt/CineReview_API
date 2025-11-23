using CineReview.DTOs;
using CineReview.Service.Avaliacao;
using CineReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _service;

        public AvaliacaoController(IAvaliacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Avaliar([FromBody] CriarAvaliacaoDTO dto)
        {
            try
            {
                var resultado = await _service.AvaliarAsync(dto);
                return CreatedAtAction(nameof(ListarTodas), new { id = resultado.Id }, resultado);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("midia/{id}")]
        public async Task<IActionResult> ListarPorMidia(Guid id)
        {
            return Ok(await _service.ListarPorMidiaAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            return Ok(await _service.ListarTodasAsync());
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