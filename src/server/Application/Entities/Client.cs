using System.Security.Cryptography.X509Certificates;
using System;
namespace Hz.IdentityServer.Application.Entities
{
    public class Client
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string client_name { get; set; }
        public string redirect_uri { get; set; }

        public bool ValidateRedirectUri(string uri)
        {
            return this.redirect_uri.Equals(uri);
        }

        /// <summary>
        /// 检验客户端合法性
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        /// <returns></returns>
        public bool CheckClient(string client_id, string client_secret)
        {
            return this.client_id.Equals(client_id) && this.client_secret.Equals(client_secret);
        }

        public static Client AdminClient()
        {
            return new Client {
                client_id = "AdminOrg",
                client_secret = "123456",
                client_name = $"机构{DateTime.Now.Ticks}",
                redirect_uri = $"https://www.baidu.com"
            };
        }
    }
}