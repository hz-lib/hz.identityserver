namespace Hz.IdentityServer.Common
{
    /// <summary>
    /// authorization_code,
    /// refresh_token,
    /// client_credentials,
    /// password
    /// </summary>
    public static class GrantType
    {
        public const string Code = "authorization_code";
        public const string RefreshToken = "refresh_token";
        public const string Client = "client_credentials";
        public const string Password = "password";
    }
}