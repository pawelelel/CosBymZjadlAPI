using Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDatabase, Database.Database>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure cors
builder.Services.AddCors(c =>
{
	c.AddPolicy("Policy", policy => policy
		.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

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
