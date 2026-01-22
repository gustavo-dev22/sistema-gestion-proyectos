using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Modelo
{
    public class UsuarioEquipo
    {
        public int IdUsuarioEquipo { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdEquipo { get; set; }
        public Equipo Equipo { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
