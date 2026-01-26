using Microsoft.IdentityModel.Tokens;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;
using SistemaGestionProyectos.DTOs.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaGestionProyectos.Services.Login
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("SASI");

            var sasiRequest = new SasiLoginRequest
            {
                UserName = request.Usuario,
                Password = request.Contrasena
            };

            var httpResponse = await client.PostAsJsonAsync(
                "SASI/api/Auth/login",
                sasiRequest
            );

            var sasiResponse = await httpResponse.Content
                .ReadFromJsonAsync<SasiLoginResponse>();

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = sasiResponse?.Message ?? "Credenciales inválidas",
                    IntentosRestantes = sasiResponse?.IntentosRestantes
                };
            }

            if (sasiResponse.Usuario == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "No se pudo obtener la información del usuario desde SASI"
                };
            }

            // 🔎 Buscar este sistema en particular
            var sistemaProyectos = sasiResponse.Usuario.Sistemas
                .FirstOrDefault(s => s.Nombre.Contains("Gestion de Proyectos"));

            if (sistemaProyectos == null)
                throw new UnauthorizedAccessException(
                    "No tiene acceso al Sistema de Gestión de Proyectos"
                );

            var rolPrincipal = sistemaProyectos.Roles
                .FirstOrDefault(r => r.EsPrincipal);

            return new LoginResponse
            {
                Token = sasiResponse.Token,
                NombreCompleto = sasiResponse.Usuario.NombreCompleto,
                Correo = sasiResponse.Usuario.Email,
                Rol = rolPrincipal?.NombreRol ?? "SIN_ROL"
            };
        }
    }
}
