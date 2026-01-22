using Microsoft.EntityFrameworkCore;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;
using SistemaGestionProyectos.Infraestructura.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Infraestructura.Repositories
{
    public class ProyectoEquipoRepository : IGenericoRepository<ProyectoEquipo>, IProyectoEquipoRepository
    {
        private readonly ProyectosDbContext _context;

        public ProyectoEquipoRepository(ProyectosDbContext context)
        {
            _context = context;
        }
        public async Task ActualizarAsync(ProyectoEquipo proyectoEquipo)
        {
            _context.ProyectosEquipos.Update(proyectoEquipo);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(ProyectoEquipo proyectoEquipo)
        {
            _context.ProyectosEquipos.Add(proyectoEquipo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var proyectoEquipo = await _context.ProyectosEquipos.FindAsync(id);
            if (proyectoEquipo != null)
            {
                _context.ProyectosEquipos.Remove(proyectoEquipo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Equipo>> ObtenerEquiposPorProyectoAsync(int idProyecto)
        {
            return await _context.Equipos
                        .Include(e => e.ProyectoEquipos)  // Cargar solo la tabla intermedia
                        .Where(e => e.ProyectoEquipos.Any(pe => pe.IdProyecto == idProyecto))
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<ProyectoEquipo> ObtenerPorIdAsync(int id)
        {
            return await _context.ProyectosEquipos.FindAsync(id);
        }

        public async Task<IEnumerable<ProyectoEquipo>> ObtenerPorProyectoIdAsync(int idProyecto)
        {
            return await _context.ProyectosEquipos
                            .Where(pe => pe.IdProyecto == idProyecto)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ProyectoEquipo>> ObtenerTodosAsync()
        {
            return await _context.ProyectosEquipos.ToListAsync();
        }
    }
}
