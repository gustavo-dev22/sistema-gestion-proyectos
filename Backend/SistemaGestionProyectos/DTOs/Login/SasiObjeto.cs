namespace SistemaGestionProyectos.DTOs.Login
{
    public class SasiObjeto
    {
        public int IdObjeto { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } // Menu | Submenu | Accion
        public string Url { get; set; }
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public bool Activo { get; set; }
        public int Orden { get; set; }
        public int? IdPadre { get; set; }
    }
}
