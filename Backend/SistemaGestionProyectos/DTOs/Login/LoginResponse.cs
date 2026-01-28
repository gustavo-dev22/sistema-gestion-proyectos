namespace SistemaGestionProyectos.DTOs.Login
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? IntentosRestantes { get; set; }

        public string Token { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public SasiUsuario? Usuario { get; set; }
    }
}
