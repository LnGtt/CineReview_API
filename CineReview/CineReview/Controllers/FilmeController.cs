using CineReview.DTOs;
using CineReview.Service.Externo;
using CineReview.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilmeController : ControllerBase
    {
        private readonly IFilmeService _service;
        private readonly OmdbService _omdbService;

        public FilmeController(IFilmeService service, OmdbService omdbService)
        {
            _service = service;
            _omdbService = omdbService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarFilmeDTO dto)
        {
            try
            {
                var filme = await _service.CadastrarAsync(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = filme.Id }, filme);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarDoOmdb([FromQuery] string titulo)
        {
            try
            {
                // 1. Busca os dados na API Externa
                var dtoPronto = await _omdbService.BuscarFilmePorTituloAsync(titulo);

                if (dtoPronto == null)
                    return NotFound("Filme não encontrado no OMDb.");

                // 2. Usa o teu serviço existente para salvar no banco!
                // (Reaproveitamento de código total!)
                var filmeSalvo = await _service.CadastrarAsync(dtoPronto);

                return CreatedAtAction(nameof(BuscarPorId), new { id = filmeSalvo.Id }, filmeSalvo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao importar: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            return Ok(await _service.ListarTodosAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try { return Ok(await _service.BuscarPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [AllowAnonymous]
        [HttpGet("ranking")]
        public async Task<IActionResult> ListarRanking()
        {
            return Ok(await _service.ListarRankingAsync());
        }

        [AllowAnonymous]
        [HttpGet("buscar")]
        public async Task<IActionResult> Filtrar([FromQuery] string genero)
        {
            if (string.IsNullOrWhiteSpace(genero))
                return BadRequest("O gênero para busca é obrigatório.");

            return Ok(await _service.FiltrarPorGeneroAsync(genero));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarFilmeDTO dto)
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