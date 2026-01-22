using Microsoft.AspNetCore.Mvc;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;

namespace SistemaGestionProyectos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private readonly IGenericoRepository<Usuario> _usuarioRepository;

        public UsuariosController(IGenericoRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioRepository.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _usuarioRepository.AgregarAsync(usuario);
            return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.IdUsuario || !ModelState.IsValid)
            {
                return BadRequest();
            }
            await _usuarioRepository.ActualizarAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _usuarioRepository.EliminarAsync(id);
            return NoContent();
        }
    }
}
