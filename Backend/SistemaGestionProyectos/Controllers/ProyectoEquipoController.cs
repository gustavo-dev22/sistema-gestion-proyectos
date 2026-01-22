using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;

namespace SistemaGestionProyectos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoEquipoController : Controller
    {
        private readonly IProyectoEquipoRepository _proyectoEquipoRepository;

        public ProyectoEquipoController(IProyectoEquipoRepository proyectoEquipoRepository)
        {
            _proyectoEquipoRepository = proyectoEquipoRepository;
        }

        [HttpGet("{proyectoId}/equipos-asignados")]
        public async Task<IActionResult> ObtenerEquiposAsignados(int proyectoId)
        {
            var equiposAsignados = await _proyectoEquipoRepository.ObtenerEquiposPorProyectoAsync(proyectoId);

            if (equiposAsignados == null || !equiposAsignados.Any())
            {
                return Ok(new List<Equipo>());
            }

            return Ok(equiposAsignados);
        }
    }
}
