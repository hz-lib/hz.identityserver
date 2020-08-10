namespace Hz.IdentityServer.Models
{
    public class TokenOptions
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        /// <summary>
        /// authorization_code, refresh_token
        /// </summary>
        /// <value></value>
        public string grant_type { get; set; }

        #region authorization_code
        public string code { get; set; }
        public string redirect_uri { get; set; }
        #endregion

        #region refresh_token
        public string refresh_token { get; set; }
        #endregion
    }
}