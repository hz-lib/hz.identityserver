using System;
using Microsoft.Extensions.Caching.Distributed;

namespace Hz.IdentityServer.Common
{
    public class ClientService : IClientService
    {
        private readonly IDistributedCache _cache;

        public ClientService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 验证客户端有效性
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        /// <returns></returns>
        public bool ValidateClient(string client_id, string client_secret)
        {
            // 从存储中查询客户端，并验证是否合法
            return true;
        }

        /// <summary>
        /// 验证客户端有效性
        /// </summary>
        /// <param name="client_id"></param>
        /// <returns></returns>
        public bool ValidateClientId(string client_id)
        {
            // 查询是否存在client_id
            return true;
        }

        /// <summary>
        /// 验证Code有效性
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            if(string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            var cacheCode = _cache.GetString(CacheKeyProvider.CodeKey(code));

            return !string.IsNullOrWhiteSpace(cacheCode);
        }

        /// <summary>
        /// 验证refresh_token有效性
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public bool ValidateRefreshToken(string refreshToken)
        {
            if(string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

            var cacheRefreshToken = _cache.GetString(CacheKeyProvider.RefreshTokenKey(refreshToken));

            return !string.IsNullOrWhiteSpace(cacheRefreshToken);
        }

        /// <summary>
        /// 验证grant_type合法性
        /// </summary>
        /// <param name="grant_type"></param>
        /// <returns></returns>
        public bool ValidateGrantType(string grant_type)
        {
            if(!"refresh_token".Equals(grant_type) && !"authorization_code".Equals(grant_type) && !"client_credentials".Equals(grant_type))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证response_type合法性
        /// </summary>
        /// <param name="response_type"></param>
        /// <returns></returns>
        public bool ValidateResponseType(string response_type)
        {
            if(!response_type.IsToken() && !response_type.IsCode())
            {
                return false;
            }

            return true;
        }
    }
}