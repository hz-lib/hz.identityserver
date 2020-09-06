using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyAuthorizationHandler(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, MyAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MyAuthorizationHandler2>();

            return services;
        }
    }
}