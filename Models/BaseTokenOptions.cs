namespace Hz.IdentityServer.Models
{
    public class BaseTokenOptions
    {
        /// <summary>
        /// authorization_code,
        /// refresh_token,
        /// client_credentials,
        /// password
        /// </summary>
        /// <value></value>
        public string grant_type { get; set; }
        public string client_id { get; set; }
    }
}