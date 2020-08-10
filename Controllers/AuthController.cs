using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Hz.IdentityServer.Models;
using Hz.IdentityServer.Common;

namespace Hz.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IClientService _clientService;

        public AuthController(ILogger<AuthController> logger, IDistributedCache cache, IClientService clientService)
        {
            _logger = logger;
            _cache = cache;
            _clientService = clientService;
        }

        public IActionResult Authorize([FromHeader]AuthorizeOptions options)
        {
            // 验证clientid
            if(!_clientService.ValidateClientId(options.client_id) || !_clientService.ValidateResponseType(options.response_type)) 
            {
                return RedirectToAction("Error", "Home");
            }

            ViewData["orgName"] = "xxx机构";
            ViewData["options"] = options;
            return View();
        }

        [HttpPost]
        public IActionResult Submit([FromBody]LoginModel model)
        {
            if(model.account == "admin" && model.passwd == "123456")
            {
                var code =  Guid.NewGuid().ToString("N");

                if (model.response_type.IsToken()) {
                    _cache.SetString(CacheKeyProvider.TokenKey(code), model.account, new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    });
                } else {
                    _cache.SetString(CacheKeyProvider.CodeKey(code), model.account, new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }

                return Ok(code);
            }
            else {
                return RedirectToAction("Error","Home");
            }
        }

        [HttpGet]
        public IActionResult Token([FromHeader]TokenOptions options)
        {
            if(!_clientService.ValidateClient(options.client_id, options.client_secret))
            {
                return Redirect(TokenResult.BuildUrl(TokenResult.Error("error client"),options.redirect_uri));
            }

            if(!_clientService.ValidateGrantType(options.grant_type))
            {
                return Redirect(TokenResult.BuildUrl(TokenResult.Error("error grant_type"),options.redirect_uri));
            }

            var user = "";

            if("authorization_code".Equals(options.grant_type))
            {
                if(!_clientService.ValidateCode(options.code))
                {
                    return Redirect(TokenResult.BuildUrl(TokenResult.Error("error code"),options.redirect_uri));
                }
                else
                {
                    user = _cache.GetString(CacheKeyProvider.CodeKey(options.code));
                    _cache.Remove(CacheKeyProvider.CodeKey(options.code));
                }
            } 
            else if("refresh_token".Equals(options.grant_type))
            {
                if(!_clientService.ValidateRefreshToken(options.refresh_token))
                {
                    return Redirect(TokenResult.BuildUrl(TokenResult.Error("error refresh token"),options.redirect_uri));
                }
                else
                {
                    user = _cache.GetString(CacheKeyProvider.RefreshTokenKey(options.refresh_token));
                    _cache.Remove(CacheKeyProvider.RefreshTokenKey(options.refresh_token));
                }
            }
            else
            {
                // 客户端用户
                user = options.client_id;
            }

            var token = Guid.NewGuid().ToString("N");
            var refreshToken = Guid.NewGuid().ToString("N");
            var tokenExpiresIn = 60*60*24; // 1天
            var refreshTokenExpiresIn = 60*60*24*365; // 1年

            var tokenResult = new TokenResult {
                access_token = token,
                refresh_token = refreshToken,
                expires_in = tokenExpiresIn
            };

            // 将access_token,refresh_token加入缓存
            _cache.SetString(CacheKeyProvider.TokenKey(token), user, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenExpiresIn)
            });

            _cache.SetString(CacheKeyProvider.RefreshTokenKey(refreshToken), user, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(refreshTokenExpiresIn)
            });

            return Redirect(TokenResult.BuildUrl(tokenResult, options.redirect_uri));
        }
    }
}