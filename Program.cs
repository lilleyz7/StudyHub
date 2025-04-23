using Microsoft.EntityFrameworkCore;
using StudyHub.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

var environmentProtection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if(environmentProtection is null) throw new Exception("Unable to load environment");

if(environmentProtection == "Development")
{
    var sqliteConnection = "Source Data=app.dev.db";
    builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite(sqliteConnection));
} else if(environmentProtection == "Production")
{
    var postgresConnection = Environment.GetEnvironmentVariable("PRODUCTION_DATABASE");
}
else
{
    throw new Exception("Invalid environment settings");
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
