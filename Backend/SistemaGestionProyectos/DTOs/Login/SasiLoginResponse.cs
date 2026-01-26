using System.Text.Json.Serialization;

namespace SistemaGestionProyectos.DTOs.Login
{
    public class SasiLoginResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("bloqueado")]
        public bool Bloqueado { get; set; }

        [JsonPropertyName("intentosFallidos")]
        public int IntentosFallidos { get; set; }

        [JsonPropertyName("intentosRestantes")]
        public int IntentosRestantes { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

        [JsonPropertyName("usuario")]
        public SasiUsuario Usuario { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
