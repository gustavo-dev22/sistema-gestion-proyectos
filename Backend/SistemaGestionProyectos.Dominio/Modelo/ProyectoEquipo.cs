using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Modelo
{
    public class ProyectoEquipo
    {
        public int IdProyectoEquipo { get; set; }
        public int IdProyecto { get; set; }
        public Proyecto Proyecto { get; set; }
        public int IdEquipo { get; set; }
        public Equipo Equipo { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
