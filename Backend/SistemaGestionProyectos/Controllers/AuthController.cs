using Microsoft.AspNetCore.Mvc;
using SistemaGestionProyectos.DTOs.Login;
using SistemaGestionProyectos.Services.Login;

namespace SistemaGestionProyectos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);

                // Siempre devolver 200, con Success=true/false
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Solo error real de servidor
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error inesperado. Intenta más tarde.",
                    Detalle = ex.Message
                });
            }
        }
    }
}
