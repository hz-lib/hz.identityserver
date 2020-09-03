using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Security.Claims;


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

    /// <summary>
    /// 自定义生成jwttoken
    /// 参考：https://stackoverflow.com/questions/38725038/c-sharp-how-to-verify-signature-on-jwt-token
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public string GenerateJwtToken(string id, string username)
    {
      // header
      var jwtHeader = new {
        typ = "JWT",
        alg = "HS256"
      };
      var jwtHeaderBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(jwtHeader);
      var jwtHeaderBase64 = Base64UrlEncode(jwtHeaderBytes);
      
      // payload
      
      // 使用微软定义的Claim，可以直接配合微软的中间件直接使用
      // var jwtPayload = new List<Claim> {
      //   new Claim("id", id),
      //   new Claim("username", username)
      // };

      // 可以使用自定义的模型，但是在客户端使用时就需要重写中间件
      var jwtPayload = new {
        id = id,
        username = username
      };

      var jwtPayloadBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(jwtPayload);
      var jwtPayloadBase64 = Base64UrlEncode(jwtPayloadBytes);

      // signature
      var security = "abc!@#123";

      var alg = new HMACSHA256(Encoding.UTF8.GetBytes(security));
      var bytesToSign = Encoding.UTF8.GetBytes($"{jwtHeaderBase64}.{jwtPayloadBase64}");
      var hash = alg.ComputeHash(bytesToSign);
      var jwtSignBase64 = Base64UrlEncode(hash);

      // token
      return $"{jwtHeaderBase64}.{jwtPayloadBase64}.{jwtSignBase64}";
    }

    private string Base64UrlEncode(byte[] input)
    {
      var output = Convert.ToBase64String(input);
      output = output.Split('=')[0].Replace('+','-').Replace('/','_');
      return output;
    }
  }
}