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

            var httpResponse = await client.PostAsJsonAsync(
                "SASI/api/Auth/login",
                new SasiLoginRequest
                {
                    UserName = request.Usuario,
                    Password = request.Contrasena
                });

            var sasiResponse = await httpResponse.Content
                .ReadFromJsonAsync<SasiLoginResponse>();

            // 1️⃣ Error técnico
            if (!httpResponse.IsSuccessStatusCode || sasiResponse == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Error al comunicarse con el servicio de autenticación"
                };
            }

            // 2️⃣ Login FALLIDO → manejar SOLO por Código
            if (!sasiResponse.Success)
            {
                // 🔒 Cuenta bloqueada
                if (sasiResponse.Codigo == "CUENTA_BLOQUEADA")
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = sasiResponse.Message ?? "La cuenta se encuentra bloqueada"
                    };
                }

                // 🔑 Password incorrecta → SOLO aquí mostrar intentos
                if (sasiResponse.Codigo == "PASSWORD_INCORRECTA")
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Contraseña incorrecta",
                        IntentosRestantes = sasiResponse.IntentosRestantes
                    };
                }

                // 👤 Usuario no existe / credenciales inválidas
                if (sasiResponse.Codigo == "CREDENCIALES_INCORRECTAS")
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Credenciales incorrectas"
                    };
                }

                // fallback defensivo
                return new LoginResponse
                {
                    Success = false,
                    Message = sasiResponse.Message ?? "Error de autenticación"
                };
            }

            // 3️⃣ Login OK → validar acceso a sistemas
            var sistemas = sasiResponse.Usuario?.Sistemas;

            if (sistemas == null || !sistemas.Any())
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "No tiene sistemas asignados"
                };
            }

            var sistemaProyectos = sistemas
                .FirstOrDefault(s => s.Nombre.Contains("Gestion de Proyectos"));

            if (sistemaProyectos == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "No tiene acceso al Sistema de Gestión de Proyectos"
                };
            }

            // 4️⃣ Login exitoso
            var rolPrincipal = sistemaProyectos.Roles?
                .FirstOrDefault(r => r.EsPrincipal);

            return new LoginResponse
            {
                Success = true,
                Token = sasiResponse.Token,
                NombreCompleto = sasiResponse.Usuario.NombreCompleto,
                Correo = sasiResponse.Usuario.Email,
                Rol = rolPrincipal?.NombreRol ?? "SIN_ROL",
                Usuario = sasiResponse.Usuario
            };
        }
    }
}
