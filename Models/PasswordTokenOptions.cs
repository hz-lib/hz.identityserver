namespace Hz.IdentityServer.Models
{
    /// <summary>
    /// 密码式请求模型
    /// </summary>
    public class PasswordTokenOptions : BaseTokenOptions
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}