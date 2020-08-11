namespace Hz.IdentityServer.Models
{
    public class TokenOptions : BaseTokenOptions
    {
        public string client_secret { get; set; }

        #region authorization_code
        public string code { get; set; }
        public string redirect_uri { get; set; }
        #endregion

        #region refresh_token
        public string refresh_token { get; set; }
        #endregion
    }
}