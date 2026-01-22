using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;
using SistemaGestionProyectos.DTOs.Asignacion;

namespace SistemaGestionProyectos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : Controller
    {
        private readonly IGenericoRepository<Proyecto> _proyectoRepository;
        private readonly IGenericoRepository<ProyectoEquipo> _proyectoEquipoRepository;
        private readonly IProyectoEquipoRepository _proyectoEquipoRepository1;

        public ProyectosController(IGenericoRepository<Proyecto> proyectoRepository, IGenericoRepository<ProyectoEquipo> proyectoEquipoRepository, IProyectoEquipoRepository proyectoEquipoRepository1)
        {
            _proyectoRepository = proyectoRepository;
            _proyectoEquipoRepository = proyectoEquipoRepository;
            _proyectoEquipoRepository1 = proyectoEquipoRepository1;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProyectos()
        {
            try
            {
                var proyectos = await _proyectoRepository.ObtenerTodosAsync();

                // Obtener los equipos asignados a cada proyecto
                foreach (var proyecto in proyectos)
                {
                    proyecto.Equipos = (await _proyectoEquipoRepository1.ObtenerEquiposPorProyectoAsync(proyecto.IdProyecto)).ToList();
                }

                return Ok(proyectos);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error al obtener proyectos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProyecto(int id)
        {
            var proyecto = await _proyectoRepository.ObtenerPorIdAsync(id);
            return proyecto == null ? NotFound() : Ok(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProyecto([FromBody] Proyecto proyecto)
        {
            await _proyectoRepository.AgregarAsync(proyecto);
            return CreatedAtAction(nameof(ObtenerProyecto), new { id = proyecto.IdProyecto }, proyecto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProyecto(int id, [FromBody] Proyecto proyecto)
        {
            if (id != proyecto.IdProyecto) return BadRequest("El ID no coincide.");
            await _proyectoRepository.ActualizarAsync(proyecto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProyecto(int id)
        {
            try
            {
                await _proyectoRepository.EliminarAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al eliminar el proyecto." });
            }
        }

        [HttpPost("{proyectoId}/asignar")]
        public async Task<IActionResult> AsignarUsuariosYEquipos(int proyectoId, [FromBody] AsignacionRequest request)
        {
            var proyecto = await _proyectoRepository.ObtenerPorIdAsync(proyectoId);
            if (proyecto == null)
            {
                return NotFound("Proyecto no encontrado");
            }

            // Obtener los equipos actualmente asignados al proyecto
            var equiposAsignadosActuales = await _proyectoEquipoRepository1.ObtenerPorProyectoIdAsync(proyectoId); // Método que retorna los equipos asignados

            var equiposActualesIds = equiposAsignadosActuales.Select(e => e.IdEquipo).ToList();

            // Identificar equipos para agregar y eliminar
            var equiposParaAgregar = request.Equipos.Except(equiposActualesIds).ToList();
            var equiposParaEliminar = equiposActualesIds.Except(request.Equipos).ToList();

            // Asignar equipos
            foreach (var equipoId in equiposParaAgregar)
            {
                var asignacionEquipo = new ProyectoEquipo
                {
                    IdProyecto = proyectoId,
                    IdEquipo = equipoId
                };
                await _proyectoEquipoRepository.AgregarAsync(asignacionEquipo);
            }

            // Eliminar equipos no deseados
            foreach (var equipoId in equiposParaEliminar)
            {
                var asignacionAEliminar = equiposAsignadosActuales.FirstOrDefault(e => e.IdEquipo == equipoId);
                if (asignacionAEliminar != null)
                {
                    await _proyectoEquipoRepository.EliminarAsync(asignacionAEliminar.IdProyectoEquipo);
                }
            }

            return Ok(new { mensaje = "Asignaciones guardadas con éxito" });
        }
    }
}
