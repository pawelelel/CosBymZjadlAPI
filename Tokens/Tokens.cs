using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users;

namespace Tokens;

public class Tokens : ITokens
{
	private readonly Settings settings;
	private const string MyIssuer = "http://mysite.com";
	private const string MyAudience = "http://myaudience.com";

	public Tokens(IOptions<Settings> settings)
	{
		this.settings = settings.Value;
	}

	public string GenerateToken(User user, bool isAdmin)
	{
		string secret = settings.Secret;
		SymmetricSecurityKey mySecurityKey = new(Encoding.ASCII.GetBytes(secret));
		JwtSecurityTokenHandler tokenHandler = new();
		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim("id", user.Id.ToString())
			}),
			Expires = DateTime.Now.AddDays(30),
			Issuer = MyIssuer,
			Audience = MyAudience,
			SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
		};

		if (isAdmin)
		{
			tokenDescriptor.Subject.AddClaim(new Claim("Admin", "true"));
		}

		SecurityToken secToken = tokenHandler.CreateToken(tokenDescriptor);
		string token = tokenHandler.WriteToken(secToken);
		return token;
	}

	public bool ValidateToken(string token)
	{
		string secret = settings.Secret;
		SymmetricSecurityKey mySecurityKey = new(Encoding.ASCII.GetBytes(secret));
		JwtSecurityTokenHandler tokenHandler = new();
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = MyIssuer,
				ValidAudience = MyAudience,
				IssuerSigningKey = mySecurityKey
			}, out SecurityToken _);

			return true;
		}
		catch (Exception e)
		{
			return false;
		}
	}

	public bool ValidateUserId(string token, int userId)
	{
		string secret = settings.Secret;
		SymmetricSecurityKey mySecurityKey = new(Encoding.ASCII.GetBytes(secret));
		JwtSecurityTokenHandler tokenHandler = new();
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = MyIssuer,
				ValidAudience = MyAudience,
				IssuerSigningKey = mySecurityKey
			}, out SecurityToken _);

			JwtSecurityToken securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			string stringClaimValue = securityToken.Claims.First().Value;
			
			bool valid = stringClaimValue == userId.ToString();

			return valid;
		}
		catch (Exception e)
		{
			return false;
		}
	}

	public bool ValidateIsAdmin(string token)
	{
		string secret = settings.Secret;
		SymmetricSecurityKey mySecurityKey = new(Encoding.ASCII.GetBytes(secret));
		JwtSecurityTokenHandler tokenHandler = new();
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = MyIssuer,
				ValidAudience = MyAudience,
				IssuerSigningKey = mySecurityKey
			}, out SecurityToken _);

			JwtSecurityToken securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			return securityToken.Claims.ToList()[1].Value == "true";
		}
		catch (Exception e)
		{
			return false;
		}
	}
}
