using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionProyectos.Dominio.Repositories
{
    public interface IProyectoRepository
    {
        Task<bool> TieneEquiposAsignadosAsync(int proyectoId);
    }
}
