using Blanketomat.API.DTOs.LoginDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blanketomat.API.Helper;

public static class TokenManager
{
    public static string CreateToken(LoginRequestDTO user, string secretKey)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, user.AccountType)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}