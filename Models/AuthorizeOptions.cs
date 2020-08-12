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
        /// <summary>
        /// 可选
        /// </summary>
        /// <value></value>
        public string scope { get; set; }
        public string state { get; set; }

        // 与客户端信息一起保存到数据库,第三方客户端不需要传
        public string redirect_uri { get; set; }
    }
}