using System.Data.SqlClient;
using Meals;
using Microsoft.Extensions.Configuration;

namespace MealsDatabase;

public class MealsDatabase : IMealsDatabase
{
	private readonly IConfiguration configuration;
	public MealsDatabase(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public void AddMeal(Meal meal)
	{
		throw new NotImplementedException();
	}

	public void DeleteMeal(Meal meal)
	{
		throw new NotImplementedException();
	}

	public List<Meal> FindMeal(string name)
	{
		throw new NotImplementedException();
	}

	public Meal GetMeal(int id)
	{
		throw new NotImplementedException();
	}

	public Meal GetRandomMeal(RandomMealCriteria randomMealCriteria)
	{
		throw new NotImplementedException();
	}
}