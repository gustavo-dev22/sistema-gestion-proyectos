using Microsoft.EntityFrameworkCore;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;
using SistemaGestionProyectos.Infraestructura.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SistemaGestionProyectos.Infraestructura.Repositories
{
    public class UsuarioRepository : IUsuarioRepository, IGenericoRepository<Usuario>
    {
        private readonly ProyectosDbContext _context;

        public UsuarioRepository(ProyectosDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Usuario entidad)
        {
            _context.Usuarios.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Usuario entidad)
        {
            await _context.Usuarios.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> ObtenerPorUsuarioAsync(string usuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuario);
        }

        public async Task<bool> ValidarContrasenaAsync(string contrasenaHash, string contrasena)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(contrasena, contrasenaHash));
        }
    }
}
