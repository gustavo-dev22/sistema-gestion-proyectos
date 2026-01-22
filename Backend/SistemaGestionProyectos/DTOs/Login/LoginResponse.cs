namespace SistemaGestionProyectos.DTOs.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}
