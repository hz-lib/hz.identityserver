namespace Hz.IdentityServer.Models
{
    public class TokenResult
    {
        public string access_token { get; set; }
        /// <summary>
        /// 一般固定为 Bearer 
        /// </summary>
        /// <value></value>
        public string token_type { get; set; } = "Bearer";
        /// <summary>
        /// 可选
        /// </summary>
        /// <value></value>
        public int expires_in { get; set; }
        /// <summary>
        /// 可选
        /// </summary>
        /// <value></value>
        public string refresh_token { get; set; }
        // /// <summary>
        // /// 可选
        // /// </summary>
        // /// <value></value>
        // public string scope { get; set; }
        public string userid { get; set; }
    }
}