using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Modelo
{
    public class Equipo
    {
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        [JsonIgnore]
        public IEnumerable<ProyectoEquipo> ProyectoEquipos { get; set; } = new List<ProyectoEquipo>();

        // Relación con los proyectos (a través de ProyectoEquipo)
        public IEnumerable<Proyecto> Proyectos => ProyectoEquipos?.Select(pe => pe.Proyecto) ?? Enumerable.Empty<Proyecto>();

        public IEnumerable<Usuario> Usuarios { get; set; } // Relación con usuarios
    }
}
