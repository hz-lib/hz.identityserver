namespace Hz.IdentityServer.Models
{
    public class ClientOptions : BaseTokenOptions
    {
        public string client_secret { get; set; }

        public bool ValidateGrantType()
        {
            return "client_credentials".Equals(this.grant_type);
        }
    }
}