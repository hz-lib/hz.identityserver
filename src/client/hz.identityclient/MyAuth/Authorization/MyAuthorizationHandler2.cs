using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class MyAuthorizationHandler2 : AuthorizationHandler<MyAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
        {
            Console.WriteLine("MyAuthorizationHandler2 Handle Requirement");
            var result = context.HasFailed;
            if(requirement._funcs.Any(f => context.User.HasClaim(c => c.Value == f)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}