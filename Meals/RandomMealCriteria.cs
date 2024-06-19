namespace Meals;

public class RandomMealCriteria
{
	public bool Breakfast { get; set; }

	public bool Dinner { get; set; }

	public bool Dessert { get; set; }

	public bool Other { get; set; }

	public bool IncludeLastMeals { get; set; }

	public bool IncludeFrequencyOfMeals { get; set; }

	public bool OnlyFavouriteMeals { get; set; }

	public Difficulty Difficulty { get; set; }
}