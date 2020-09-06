using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication
{
    public class MyAuthenticationHandler : AuthenticationHandler<MyAuthenticationSchemeOptions>
    {
        public MyAuthenticationHandler(IOptionsMonitor<MyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            var attributes = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>();
            if (endpoint is null || attributes != null) 
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization");
            // 这里我是用微软的claim结构生成的jwt，所以可以这么用
            // 如果你是自定义的，需要注意一下
            var idClaim = Context.User.FindFirst("id");
            if (idClaim is null) {
                return Task.FromResult(AuthenticateResult.Fail("无效token"));
            }
            var id = idClaim.Value;
            var claimsPrincipal = TransClaimsPrincipal(id);
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }

        private ClaimsPrincipal TransClaimsPrincipal(string id)
        {
            var claims = new List<Claim>() {
                new Claim("id", id),
                new Claim("Name", "name222")
            };
            claims.AddRange(Enumerable
                .Range(1, 10)
                .Select(i => new Claim($"funcs",$"func{i}")));

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            return new ClaimsPrincipal(claimsIdentity);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var fail = authResult?.Failure;
            if (fail != null)
            {
                Response.StatusCode = 401;
                return;
            }

            await base.HandleChallengeAsync(properties);
        }
    }
}