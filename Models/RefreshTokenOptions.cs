namespace Hz.IdentityServer.Models
{
    public class RefreshTokenOptions
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        /// <summary>
        /// 可选
        /// </summary>
        /// <value></value>
        public string scope { get; set; }

        public bool ValidateGrantType()
        {
            return "refresh_token".Equals(this.grant_type);
        }
    }
}