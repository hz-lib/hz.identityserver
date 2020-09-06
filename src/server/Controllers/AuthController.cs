using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Hz.IdentityServer.Models;
using Hz.IdentityServer.Common;
using Hz.IdentityServer.Application.Services;
using Hz.IdentityServer.Application.Entities;

namespace Hz.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IClientService _clientService;
        private readonly IAuthKeyFactory _keyFactory;

        public AuthController(
            ILogger<AuthController> logger, 
            IDistributedCache cache, 
            IClientService clientService,
            IAuthKeyFactory keyFactory)
        {
            _logger = logger;
            _cache = cache;
            _clientService = clientService;
            _keyFactory = keyFactory;
        }

        public IActionResult Authorize([FromHeader]AuthorizeOptions options)
        {
            // 验证clientid
            if(!_clientService.ValidateClientId(options.client_id) || !_clientService.ValidateResponseType(options.response_type)) 
            {
                return RedirectToAction("Error", "Home", new { msg = "not exists client or error response_type"} );
            }
            var client = Client.AdminClient();
            ViewData["orgName"] = client.client_name;
            options.redirect_uri = client.redirect_uri;
            ViewData["options"] = options;
            return View();
        }

        [HttpPost]
        public IActionResult Submit([FromBody]LoginModel model)
        {
            var adminUser = UserInfo.CreateAdminUser();
            if(adminUser.CheckUser(model.account, model.passwd))
            {
                var userid = adminUser.id;
                var code =  _keyFactory.GenerateCode();

                if (model.response_type.IsToken()) {
                    // 隐式模式
                    _cache.SetString(CacheKeyProvider.TokenKey(code), userid.ToString(), new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    });
                } else {
                    _cache.SetString(CacheKeyProvider.CodeKey(code), userid.ToString(), new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }

                return Ok(code);
            }
            else {
                return Problem("账号或密码错误！");
            }
        }

        /// <summary>
        /// 授权码模式
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CodeToken([FromBody]CodeTokenOptions options)
        {
            // validate grant_type
            if(!options.ValidateGrantType()) {
                return Json(new {
                    error = "error grant_type"
                });
            }

            var client = Client.AdminClient();
            // validate client
            if(client is null) {
                return Json(new {
                    error = "invalid client"
                });
            }

            // validate redirect_uri
            if (!client.ValidateRedirectUri(options.redirect_uri)) {
                return Json(new {
                    error = "error redirect_uri"
                });
            }
            // validate code
            if (!_clientService.ValidateCode(options.code)) {
                return Json(new {
                    error = "invalid code"
                });
            }

            // return
            var userid = _clientService.GetUserIdByCode(options.code);
            
            var tokenResult = HandleToken(userid);

            return Json(tokenResult);
        }

        /// <summary>
        /// 密码式
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PasswordToken([FromBody]PasswordTokenOptions options)
        {
            // client_id 可以没有
            // if(!_clientService.ValidateClientId(options.client_id))
            // {
            //     return Json(new { error = "invalid clientid"});
            // }
            
            if(!options.ValidateGrantType())
            {
                return Json(new { error = "error grant_type" });
            }

            var userAdmin = UserInfo.CreateAdminUser();
            if(userAdmin.CheckUser(options.username, options.password))
            {
                return Json(HandleToken(userAdmin.id.ToString()));
            }
            else
            {
                return Json(new { error = "error userinfo"});
            }
        }

        /// <summary>
        /// 客户端模式
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ClientToken([FromBody]ClientOptions options)
        {
            // validate grant_type
            if(!options.ValidateGrantType())
            {
                return Json(new { error = "error grant_type" });
            }

            // validate client
            var client = Client.AdminClient();
            if(client is null || !client.CheckClient(options.client_id, options.client_secret))
            {
                return Json(new { error = "invalid client"});
            }

            // return
            // use client_id as userid
            return Json(HandleToken(options.client_id));
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RefreshToken([FromBody]RefreshTokenOptions options)
        {
            // validate grant_type
            if(!options.ValidateGrantType()) {
                return Json(new { error = "error grant_type"});
            }

            // validate refresh_token
            if(!_clientService.ValidateRefreshToken(options.refresh_token)) {
                return Json(new { error = "invalid refresh_token"});
            }

            // return
            var userid = _clientService.GetUserIdByRefreshToken(options.refresh_token);
            return Json(HandleToken(userid));
        }

        private TokenResult HandleToken(string userid)
        {
            var token = _keyFactory.GenerateToken();
            var refreshToken = _keyFactory.GenerateToken();
            var tokenExpiresIn = 60*60*24; // 1天
            var refreshTokenExpiresIn = 60*60*24*365; // 1年

            var tokenResult = new TokenResult {
                access_token = token,
                refresh_token = refreshToken,
                expires_in = tokenExpiresIn,
                userid = userid
            };

            // 将access_token,refresh_token加入缓存
            _cache.SetString(CacheKeyProvider.TokenKey(token), userid, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenExpiresIn)
            });

            _cache.SetString(CacheKeyProvider.RefreshTokenKey(refreshToken), System.Text.Json.JsonSerializer.Serialize(tokenResult), new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(refreshTokenExpiresIn)
            });

            return tokenResult;
        }
        
        /// <summary>
        /// 获取jwttoken
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult JwtToken([FromBody]JwtOptions options)
        {
            // validate parameter
            if(string.IsNullOrWhiteSpace(options?.account) || string.IsNullOrWhiteSpace(options?.password))
            {
                return Json(new { error = "account or password is null or empty"});
            }

            // validate user
            var user = UserInfo.CreateAdminUser();
            var checkResult = user.CheckUser(options.account, options.password);

            if (!checkResult)
            {
                return Json(new { error = "account or password error"});
            }

            // generate jwt token
            var token = _keyFactory.GenerateJwtToken(user.id.ToString(), user.username);
            return Json(token);
        }
    }
}