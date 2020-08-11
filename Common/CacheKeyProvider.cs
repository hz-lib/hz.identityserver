namespace Hz.IdentityServer.Common
{
    public static class CacheKeyProvider
    {
        public const string TokenPrefix = "token:";
        public const string CodePrefix = "code:";
        public const string RefreshTokenPrefix = "refreshtoken:";

        /// <summary>
        /// 生成对应客户端token缓存key
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string TokenKey(string token)
        {
            return $"{TokenPrefix}{token}";
        }

        /// <summary>
        /// 生成对应客户端cache缓存key
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeKey(string code)
        {
            return $"{CodePrefix}{code}";
        }

        /// <summary>
        /// 生成对应客户端refreshtoken缓存key
        /// </summary>
        /// <param name="client_id"></param>
        /// <returns></returns>
        public static string RefreshTokenKey(string refreshToken)
        {
            return $"{RefreshTokenPrefix}{refreshToken}";
        }
    }
}