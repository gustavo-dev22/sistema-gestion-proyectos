using SistemaGestionProyectos.Dominio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorUsuarioAsync(string usuario);
        Task<bool> ValidarContrasenaAsync(string contrasenaHash, string contrasena);
    }
}
