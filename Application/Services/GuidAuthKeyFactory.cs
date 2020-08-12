using System;
namespace Hz.IdentityServer.Application.Services
{
  public class GuidAuthKeyFactory : IAuthKeyFactory
  {
    public string GenerateCode()
    {
      return Guid.NewGuid().ToString("N");
    }

    public string GenerateToken()
    {
      return Guid.NewGuid().ToString("N");
    }
  }
}