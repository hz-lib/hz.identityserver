namespace Hz.IdentityServer.Models
{
    public class AuthorizeOptions
    {
        /// <summary>
        /// code, token
        /// </summary>
        /// <value></value>
        public string response_type { get; set; }
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string scope { get; set; }
    }
}