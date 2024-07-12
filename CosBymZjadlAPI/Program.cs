using MealsDatabase;
using Tokens;
using UsersDatabase;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMealsDatabase, MealsDatabase.MealsDatabase>();
builder.Services.AddSingleton<IUsersDatabase, UsersDatabse>();
builder.Services.AddSingleton<ITokens, Tokens.Tokens>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfigurationSection config = builder.Configuration.GetSection("Secret");

builder.Services.Configure<Settings>(settings =>
{
	settings.Secret = config.Value;
});

// Configure cors
builder.Services.AddCors(c =>
{
	c.AddPolicy("Policy", policy => policy
		.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
