using System.Data.SqlClient;
using Meals;
using Microsoft.Extensions.Configuration;

namespace Database
{
	public class Database : IDatabase
	{
		private readonly IConfiguration configuration;
		public Database(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public void AddMeal(Meal meal)
		{
			throw new NotImplementedException();
			string connectionString = configuration.GetConnectionString("sql");
			
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "INSERT INTO [dbo].[Books]" +
					                      "([TitleOrginal], [LanguageOrginal], [Translation], [Title], [Author], [Country], [Language], [Illustrations], [Type], [Pages], [PublishingHouse], [ISBN])" +
					                      "OUTPUT inserted.Id " +
					                      "VALUES" +
					                      "(@TitleOrginal, @LanguageOrginal, @Translation, @Title, @Author, @Country, @Language, @Illustrations, @Type, @Pages, @PublishingHouse, @ISBN)";


					command.Parameters.AddWithValue("TitleOrginal", book.TitleOrginal switch
					{
						"" => DBNull.Value,
						_ => book.TitleOrginal
					});
					command.Parameters.AddWithValue("LanguageOrginal", book.LanguageOrginal switch
					{
						"" => DBNull.Value,
						_ => book.LanguageOrginal
					});
					command.Parameters.AddWithValue("Translation", book.Translation switch
					{
						"" => DBNull.Value,
						_ => book.Translation
					});

					command.Parameters.AddWithValue("Title", book.Title);
					command.Parameters.AddWithValue("Author", book.Author);
					command.Parameters.AddWithValue("Country", book.Country);
					command.Parameters.AddWithValue("Language", book.Language);

					command.Parameters.AddWithValue("Illustrations", book.Illustrations switch
					{
						"" => DBNull.Value,
						_ => book.Illustrations
					});

					command.Parameters.AddWithValue("Type", book.Type);
					command.Parameters.AddWithValue("Pages", book.Pages);
					command.Parameters.AddWithValue("PublishingHouse", book.PublishingHouse);
					command.Parameters.AddWithValue("ISBN", book.ISBN);

					object id = command.ExecuteScalar();

					book.Id = Convert.ToInt32(id);
				}

				connection.Close();
			}

			return book;
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
}
