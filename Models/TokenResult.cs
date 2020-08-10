namespace Hz.IdentityServer.Models
{
    public class TokenResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        /// <summary>
        /// 为什么要这个？不能直接使用access_token刷新？
        /// </summary>
        /// <value></value>
        public string refresh_token { get; set; }
        public string error { get; set; }

        public static TokenResult Error(string msg) {
            return new TokenResult {
                error = msg
            };
        }

        /// <summary>
        /// 生成回调地址，回调方需要反序列成TokenResult格式
        /// </summary>
        /// <param name="result"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        public static string BuildUrl(TokenResult result, string callbackUrl) {
            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result);
            return $"{callbackUrl}?data={jsonResult}";
        }
    }
}