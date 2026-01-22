using SistemaGestionProyectos.DTOs.Login;

namespace SistemaGestionProyectos.Services.Login
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
