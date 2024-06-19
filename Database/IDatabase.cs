using Meals;

namespace Database
{
	public interface IDatabase
	{
		void AddMeal(Meal meal);

		void DeleteMeal(Meal meal);

		List<Meal> FindMeal(string name);

		Meal GetMeal(int id);

		Meal GetRandomMeal(RandomMealCriteria randomMealCriteria);
	}
}
