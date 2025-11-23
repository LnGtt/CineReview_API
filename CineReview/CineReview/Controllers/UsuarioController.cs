using CineReview.DTOs;
using CineReview.Service.Usuario;
using CineReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CriarUsuarioDTO dto)
        {
            try
            {
                var usuario = await _service.CadastrarAsync(dto);
                return CreatedAtAction(nameof(ListarTodos), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO dto)
        {
            try { return Ok(await _service.LoginAsync(dto)); }
            catch (Exception ex) { return Unauthorized(ex.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            return Ok(await _service.ListarTodosAsync());
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
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioDTO dto)
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