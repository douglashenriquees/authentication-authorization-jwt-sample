using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AuthSample.Models;

namespace AuthSample.Settings;

public static class TokenService
{
	public static string GenerateToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var key = Encoding.ASCII.GetBytes(Config.Secret);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, user.Role)
			}),

			Expires = DateTime.UtcNow.AddMinutes(30),

			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}