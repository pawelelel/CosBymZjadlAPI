using System.Data.SqlClient;
using Users;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace UsersDatabase;

public class UsersDatabse : IUsersDatabase
{
	private readonly IConfiguration configuration;
	private readonly string? connectionString;
	public UsersDatabse(IConfiguration configuration)
	{
		this.configuration = configuration;
		connectionString = configuration.GetSection("ConnectionStrings").GetSection("Library").Value;
	}

	string GenerateHash(string password)
	{
		byte[] array = Encoding.ASCII.GetBytes(password);
		HashAlgorithm sha = SHA256.Create();
		byte[] resultByte = sha.ComputeHash(array);
		string result = Convert.ToBase64String(resultByte);
		return result;
	}

	public User GetUser(int id)
	{
		User u;
		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "select Id, Name, Email from Users where Id = @id";

				command.Parameters.AddWithValue("id", id);

				SqlDataReader reader = command.ExecuteReader();
				reader.Read();

				if (reader.HasRows)
				{
					u = new()
					{
						Id = reader.GetInt32("id"),
						Name = reader.GetString("name"),
						Email = reader.GetString("email")
					};
				}
				else
				{
					reader.Close();
					connection.Close();
					return null;
				}

				reader.Close();
			}
			connection.Close();
		}

		return u;
	}

	public User UpdateUser(User user)
	{
		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "update Users set Name = @name, Email = @email where Id = @id";

				command.Parameters.AddWithValue("id", user.Id);
				command.Parameters.AddWithValue("name", user.Name);
				command.Parameters.AddWithValue("email", user.Email);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}

		return user;
	}

	public void DeleteUser(int id)
	{
		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "delete * from Users where Id = @id";

				command.Parameters.AddWithValue("id", id);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}

		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "delete * from Admins where UserId = @id";

				command.Parameters.AddWithValue("id", id);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public User AddUser(User user)
	{
		user.Password = GenerateHash(user.Password);

		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "insert into Users (Name, Email, Password) values (@name, @email, @password)";

				command.Parameters.AddWithValue("name", user.Name);
				command.Parameters.AddWithValue("email", user.Email);
				command.Parameters.AddWithValue("password", user.Password);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}

		return user;
	}

	public void AddAdmin(int id)
	{
		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "insert into Admins values @id";

				command.Parameters.AddWithValue("id", id);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public void DeleteAdmin(int id)
	{
		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "delete * from Admins where UserId = @id";

				command.Parameters.AddWithValue("id", id);

				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public User Login(LoginRequest loginRequest)
	{
		string hash = GenerateHash(loginRequest.Password);
		User u;

		using (SqlConnection connection = new(connectionString))
		{
			connection.Open();

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				command.CommandText = "select Id, Name, Email from Users where Email = @email and Password = @password";
				
				command.Parameters.AddWithValue("email", loginRequest.Email);
				command.Parameters.AddWithValue("password", hash);

				SqlDataReader reader = command.ExecuteReader();
				reader.Read();
				
				if (reader.HasRows)
				{
					u = new()
					{
						Id = reader.GetInt32("id"),
						Name = reader.GetString("name"),
						Email = reader.GetString("email")
					};
				}
				else
				{
					reader.Close();
					connection.Close();
					u = null;
				}

				reader.Close();
			}
			connection.Close();
		}

		if (u != null)
		{
			using (SqlConnection connection = new(connectionString))
			{
				connection.Open();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandType = CommandType.Text;
					command.CommandText =
						"select * from Admins where UserId = @id";

					command.Parameters.AddWithValue("id", u.Id);

					SqlDataReader reader = command.ExecuteReader();
					reader.Read();

					if (reader.HasRows)
					{
						u.IsAdmin = true;
					}
					else
					{
						u.IsAdmin = false;
					}

					reader.Close();
				}

				connection.Close();
			}
		}

		return u;
	}

	public void Logout()
	{
		throw new NotImplementedException();
	}
}
