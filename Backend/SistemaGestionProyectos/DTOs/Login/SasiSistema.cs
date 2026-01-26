namespace SistemaGestionProyectos.DTOs.Login
{
    public class SasiSistema
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<SasiRol> Roles { get; set; }
    }
}
