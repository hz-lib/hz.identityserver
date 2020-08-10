namespace Hz.IdentityServer.Common
{
    public static class ResponseType
    {
        public const string Token = "token";
        public const string Code = "code";

        /// <summary>
        /// 返回类型是否token模式
        /// </summary>
        /// <param name="response_type"></param>
        /// <returns></returns>
        public static bool IsToken(this string response_type)
        {
            return ResponseType.Token.Equals(response_type);
        }

        /// <summary>
        /// 返回类型是否code模式
        /// </summary>
        /// <param name="response_type"></param>
        /// <returns></returns>
        public static bool IsCode(this string response_type)
        {
            return ResponseType.Code.Equals(response_type);
        }
    }
}