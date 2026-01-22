using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Modelo
{
    public class Proyecto
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }

        // Propiedad para los equipos asignados
        public IEnumerable<Equipo> Equipos { get; set; } = new List<Equipo>();

        [JsonIgnore]
        public IEnumerable<ProyectoEquipo> ProyectoEquipos { get; set; } = new List<ProyectoEquipo>();
    }
}
