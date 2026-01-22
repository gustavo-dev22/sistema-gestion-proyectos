using SistemaGestionProyectos.Dominio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Repositories
{
    public interface IProyectoEquipoRepository
    {
        Task<IEnumerable<Equipo>> ObtenerEquiposPorProyectoAsync(int idProyecto);
        Task<IEnumerable<ProyectoEquipo>> ObtenerPorProyectoIdAsync(int idProyecto);
    }
}
