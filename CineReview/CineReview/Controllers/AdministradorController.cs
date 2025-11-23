using CineReview.DTOs;
using CineReview.Service.Administrador;
using CineReview.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradorController : ControllerBase
    {
        private readonly IAdministradorService _service;

        public AdministradorController(IAdministradorService service)
        {
            _service = service;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CriarAdministradorDTO dto)
        {
            try
            {
                var admin = await _service.CadastrarAsync(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = admin.Id }, admin);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDTO dto)
        {
            try
            {
                var admin = await _service.LoginAsync(dto);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
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
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAdministradorDTO dto)
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