using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Modelo
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
        public string Rol { get; set; }
        public DateTime FechaCreacion { get; set; }

        public IEnumerable<Equipo> Equipos { get; set; } // Relación con equipos
    }
}
