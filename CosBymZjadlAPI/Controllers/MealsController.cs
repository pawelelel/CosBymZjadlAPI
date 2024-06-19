using Database;
using Meals;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosBymZjadlAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MealsController : ControllerBase
{
	private IDatabase database;
	public MealsController(IDatabase database)
	{
		this.database = database;
	}

	// szukaj przepis
	// GET: api/<MealsController>
	[HttpGet("search/{name}")]
	public ActionResult Get(string name)
	{
		return Ok(database.FindMeal(name));
	}

	// wez jeden przepis po id
	// GET api/<MealsController>/5
	[HttpGet("{id}")]
	public ActionResult Get(int id)
	{
		return Ok(database.GetMeal(id));
	}

	// wez jeden przepis po id
	// GET api/<MealsController>/5
	[HttpGet]
	public ActionResult Get([FromBody] RandomMealCriteria criteria)
	{
		return Ok(database.GetRandomMeal(criteria));
	}

	// dodaj przepis
	// POST api/<MealsController>
	[HttpPost]
	public ActionResult Post([FromBody] Meal meal)
	{
		database.AddMeal(meal);
		return Created();
	}

	// edytuj przepis
	// PUT api/<MealsController>/5
	[HttpPut("{id}")]
	public ActionResult Put(int id, [FromBody] string value)
	{
		return Created();
	}

	// usun przepis
	// DELETE api/<MealsController>/5
	[HttpDelete("{id}")]
	public ActionResult Delete(int id)
	{
		Meal m = new();
		m.Id = id;
		database.DeleteMeal(m);
		return NoContent();
	}
}