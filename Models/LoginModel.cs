namespace Hz.IdentityServer.Models
{
    public class LoginModel
    {
        public string account { get; set; }
        public string passwd { get; set; }
        public string response_type { get; set; }
        public string redirect_uri { get; set; }
    }
}