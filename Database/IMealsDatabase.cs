using Meals;

namespace MealsDatabase;

public interface IMealsDatabase
{
	void AddMeal(Meal meal);

	void DeleteMeal(Meal meal);

	List<Meal> FindMeal(string name);

	Meal GetMeal(int id);

	Meal GetRandomMeal(RandomMealCriteria randomMealCriteria);
}