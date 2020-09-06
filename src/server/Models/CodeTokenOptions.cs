namespace Hz.IdentityServer.Models
{
    public class CodeTokenOptions : BaseTokenOptions
    {
        public string code { get; set; }
        public string redirect_uri { get; set; }

        public bool ValidateGrantType()
        {
            return "authorization_code".Equals(this.grant_type);
        }
    }
}