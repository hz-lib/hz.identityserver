namespace Microsoft.AspNetCore.Authorization
{
    public class MyAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] _funcs { get; }
        public MyAuthorizationRequirement(params string[] funcs)
        {
            _funcs = funcs;
        }
    }
}