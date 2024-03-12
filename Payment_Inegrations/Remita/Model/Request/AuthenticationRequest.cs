using System.Text.Json.Serialization;

namespace Remita.Model.Request
{
    public class AuthenticationRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
