using Users;

namespace Tokens;

public interface ITokens
{
	string GenerateToken(User user, bool isAdmin);

	bool ValidateToken(string token);

	bool ValidateUserId(string token, int userId);

	bool ValidateIsAdmin(string token);
}