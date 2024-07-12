using Microsoft.AspNetCore.Mvc;
using Tokens;
using Users;
using UsersDatabase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosBymZjadlAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private IUsersDatabase usersDatabase;
	private ITokens tokens;

	public UsersController(IUsersDatabase usersDatabase, ITokens tokens)
	{
		this.usersDatabase = usersDatabase;
		this.tokens = tokens;
	}

	// GET api/<UsersController>/5
	[HttpGet("{id}")]
	public ActionResult Get(int id, [FromHeader] string token)
	{
		if (!(tokens.ValidateIsAdmin(token) || tokens.ValidateUserId(token, id)))
		{
			return Unauthorized();
		}

		User u = usersDatabase.GetUser(id);
		return Ok(u);
	}

	// POST api/<UsersController>
	[HttpPost]
	public ActionResult Registration(User user)
	{
		usersDatabase.AddUser(user);
		LoginRequest loginRequest = new();
		loginRequest.Email = user.Email;
		loginRequest.Password = user.Password;
		user = usersDatabase.Login(loginRequest);
		return Created("", user);
	}

	// POST api/<UsersController>
	[HttpPost]
	public ActionResult AddAdmin([FromHeader] AddAdminRequest addAdminRequest)
	{
		if (tokens.ValidateIsAdmin(addAdminRequest.Token))
		{
			return Unauthorized();
		}

		usersDatabase.AddAdmin(addAdminRequest.NewAdminId);
		return Created();
	}

	// PUT api/<UsersController>/5
	[HttpPut("{id}")]
	public ActionResult Edit(int id, [FromHeader] string token, [FromBody] User newUser)
	{
		if (!(tokens.ValidateIsAdmin(token) || tokens.ValidateUserId(token, id)))
		{
			return Unauthorized();
		}

		newUser.Id = id;
		usersDatabase.UpdateUser(newUser);

		return Ok(newUser);
	}

	// DELETE api/<UsersController>/5
	[HttpDelete("{id}")]
	public ActionResult Delete(int id, [FromHeader] string token)
	{
		if (!(tokens.ValidateIsAdmin(token) || tokens.ValidateUserId(token, id)))
		{
			return Unauthorized();
		}
		usersDatabase.DeleteUser(id);
		return NoContent();
	}

	[HttpPost("login")]
	public ActionResult Login([FromBody] LoginRequest loginRequest)
	{
		User u;
		try
		{
			u = usersDatabase.Login(loginRequest);
			if (u == null)
			{
				return Unauthorized();
			}
		}
		catch (Exception e)
		{
			return Unauthorized();
		}

		string token = tokens.GenerateToken(u, false);
		u.Password = token;

		return Ok(u);
	}

	[HttpPost("logout")]
	public ActionResult Logout()
	{
		return Ok();
	}
}
