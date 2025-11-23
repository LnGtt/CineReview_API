using CineReview.DTOs;
using CineReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SerieController : ControllerBase
    {
        private readonly ISerieService _service;

        public SerieController(ISerieService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarSerieDTO dto)
        {
            try
            {
                var serie = await _service.CadastrarAsync(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = serie.Id }, serie);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            return Ok(await _service.ListarTodasAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                return Ok(await _service.BuscarPorIdAsync(id));
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarSerieDTO dto)
        {
            try
            {
                await _service.AtualizarAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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