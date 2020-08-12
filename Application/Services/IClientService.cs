namespace Hz.IdentityServer.Application.Services
{
    public interface IClientService
    {
         bool ValidateClientId(string client_id);
         bool ValidateCode(string code);
         bool ValidateRefreshToken(string refreshToken);
         bool ValidateResponseType(string response_type);
         string GetUserIdByCode(string code);
         string GetUserIdByRefreshToken(string refresh_token);
    }
}