namespace Hz.IdentityServer.Application.Services
{
    public interface IAuthKeyFactory
    {
         string GenerateCode();
         string GenerateToken();
    }
}