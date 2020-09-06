using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
        {
            Console.WriteLine("MyAuthorizationHandler Handle Requirement");
            var userFuncs = context.User.FindAll(c => "funcs".Equals(c.Type)).Select(c => c.Value);
            if(requirement._funcs.Length > 0)
            {
                var l1 =userFuncs.Intersect(requirement._funcs);

                if(l1.Count() != requirement._funcs.Length)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}