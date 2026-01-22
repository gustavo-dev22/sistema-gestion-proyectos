using Microsoft.AspNetCore.Mvc;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;

namespace SistemaGestionProyectos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : Controller
    {
        private readonly IGenericoRepository<Equipo> _equipoRepository;

        public EquiposController(IGenericoRepository<Equipo> equipoRepository)
        {
            _equipoRepository = equipoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipos()
        {
            var equipos = await _equipoRepository.ObtenerTodosAsync();
            return Ok(equipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipoById(int id)
        {
            var equipo = await _equipoRepository.ObtenerPorIdAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipo([FromBody] Equipo equipo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _equipoRepository.AgregarAsync(equipo);
            return CreatedAtAction(nameof(GetEquipoById), new { id = equipo.IdEquipo }, equipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipo(int id, [FromBody] Equipo equipo)
        {
            if (id != equipo.IdEquipo || !ModelState.IsValid)
            {
                return BadRequest();
            }
            await _equipoRepository.ActualizarAsync(equipo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipo(int id)
        {
            await _equipoRepository.EliminarAsync(id);
            return NoContent();
        }
    }
}
