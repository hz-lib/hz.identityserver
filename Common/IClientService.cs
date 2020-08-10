namespace Hz.IdentityServer.Common
{
    public interface IClientService
    {
         bool ValidateClient(string client_id, string client_secret);
         bool ValidateClientId(string client_id);
         bool ValidateCode(string code);
         bool ValidateRefreshToken(string refreshToken);
         bool ValidateGrantType(string grant_type);
         bool ValidateResponseType(string response_type);
    }
}