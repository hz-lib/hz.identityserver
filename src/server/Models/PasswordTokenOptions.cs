namespace Hz.IdentityServer.Models
{
    /// <summary>
    /// 密码式请求模型
    /// </summary>
    public class PasswordTokenOptions : BaseTokenOptions
    {
        public string username { get; set; }
        public string password { get; set; }
        /// <summary>
        /// 可选
        /// </summary>
        /// <value></value>
        public string scope { get; set; }

        public bool ValidateGrantType()
        {
            return "password".Equals(this.grant_type);
        }
    }
}