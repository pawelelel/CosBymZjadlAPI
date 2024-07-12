using Meals;
using MealsDatabase;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using UsersDatabase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosBymZjadlAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MealsController : ControllerBase
{
	private IMealsDatabase mealsDatabase;
	private IUsersDatabase usersDatabase;
	private ITokens tokens;

	public MealsController(IMealsDatabase mealsDatabase, IUsersDatabase usersDatabase, ITokens tokens)
	{
		this.mealsDatabase = mealsDatabase;
		this.usersDatabase = usersDatabase;
		this.tokens = tokens;
	}

	// szukaj przepis
	// GET: api/<MealsController>
	[HttpGet("search/{name}")]
	public ActionResult Get([FromHeader] string token, int userId, string name)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}
		return Ok(mealsDatabase.FindMeal(name));
	}

	// wez jeden przepis po id
	// GET api/<MealsController>/5
	[HttpGet("{id}")]
	public ActionResult Get([FromHeader] string token, int id)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}

		return Ok(mealsDatabase.GetMeal(id));
	}

	// szukaj przepis po kryteriach
	// GET api/<MealsController>/5
	[HttpGet]
	public ActionResult Get([FromHeader] string token, [FromBody] RandomMealCriteria criteria)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}

		return Ok(mealsDatabase.GetRandomMeal(criteria));
	}
	
	// dodaj przepis
	// POST api/<MealsController>
	[HttpPost]
	public ActionResult Post([FromHeader] string token, [FromBody] Meal meal)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}

		mealsDatabase.AddMeal(meal);
		return Created();
	}

	// edytuj przepis
	// PUT api/<MealsController>/5
	[HttpPut("{id}")]
	public ActionResult Put([FromHeader] string token, int id, [FromBody] string value)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}

		return Created();
	}

	// usun przepis
	// DELETE api/<MealsController>/5
	[HttpDelete("{id}")]
	public ActionResult Delete([FromHeader] string token, int id)
	{
		if (!tokens.ValidateToken(token))
		{
			return Unauthorized();
		}

		Meal m = new();
		m.Id = id;
		mealsDatabase.DeleteMeal(m);
		return NoContent();
	}
}
