using Users;

namespace UsersDatabase;

public interface IUsersDatabase
{
	User GetUser(int id);

	User UpdateUser(User user);

	void DeleteUser(int id);

	User AddUser(User user);

	void AddAdmin(int id);

	void DeleteAdmin(int id);

	User Login(LoginRequest loginRequest);

	void Logout();
}
