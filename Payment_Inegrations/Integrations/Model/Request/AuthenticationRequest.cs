using System.Text.Json.Serialization;

namespace Integrations.Model.Request
{
    public class AuthenticationRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
