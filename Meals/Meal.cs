namespace Meals;

public class Meal
{
	public int Id { get; set; }

	public string Name { get; set; }

	public Type Type { get; set; }

	public PrepareTime Time { get; set; }

	public int Calories { get; set; }

	public Difficulty Difficulty { get; set; }

	public byte[] Image { get; set; }

	public string Ingredients { get; set; }

	public string Recipe { get; set; }

	public bool IsFavourite { get; set; }
}