namespace Integrations.Model.Api.Response
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
