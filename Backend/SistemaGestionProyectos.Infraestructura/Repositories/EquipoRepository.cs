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
    public class EquipoRepository : IGenericoRepository<Equipo>
    {
        private readonly ProyectosDbContext _context;

        public EquipoRepository(ProyectosDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Equipo equipo)
        {
            _context.Equipos.Update(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Equipo equipo)
        {
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                equipo.Activo = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Equipo> ObtenerPorIdAsync(int id)
        {
            return await _context.Equipos.FindAsync(id);
        }

        public async Task<IEnumerable<Equipo>> ObtenerTodosAsync()
        {
            return await _context.Equipos.Where(e => e.Activo).ToListAsync();
        }
    }
}
