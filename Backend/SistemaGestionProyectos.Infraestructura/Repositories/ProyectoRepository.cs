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
    public class ProyectoRepository : IGenericoRepository<Proyecto>, IProyectoRepository
    {
        private readonly ProyectosDbContext _context;

        public ProyectoRepository(ProyectosDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Proyecto proyecto)
        {
            _context.Proyectos.Update(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Proyecto proyecto)
        {
            await _context.Proyectos.AddAsync(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            if (await TieneEquiposAsignadosAsync(id))
            {
                throw new InvalidOperationException("No se puede eliminar el proyecto porque tiene equipos asignados.");
            }

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Proyecto> ObtenerPorIdAsync(int id)
        {
            return await _context.Proyectos.FindAsync(id);
        }

        public async Task<IEnumerable<Proyecto>> ObtenerTodosAsync()
        {
            return await _context.Proyectos.ToListAsync();
        }

        public async Task<bool> TieneEquiposAsignadosAsync(int proyectoId)
        {
            return await _context.ProyectosEquipos.AnyAsync(pe => pe.IdProyecto == proyectoId);
        }
    }
}
