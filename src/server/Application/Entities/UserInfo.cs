using System;
namespace Hz.IdentityServer.Application.Entities
{
    public class UserInfo
    {
        public long id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public static UserInfo CreateAdminUser()
        {
            return new UserInfo {
                id = DateTime.Now.Ticks,
                username = "admin",
                password = "123456"
            };
        }

        /// <summary>
        /// 检查用户名密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUser(string account, string password)
        {
            return this.username.Equals(account) && this.password.Equals(password);
        }
    }
}