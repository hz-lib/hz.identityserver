using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class MyAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddMyAuthentication(this AuthenticationBuilder builder, Action<MyAuthenticationSchemeOptions> configureOptions = null)
        {
           return builder.AddScheme<MyAuthenticationSchemeOptions, MyAuthenticationHandler>(MyAuthenticationDefaults.Scheme, configureOptions);
        }
    }
}