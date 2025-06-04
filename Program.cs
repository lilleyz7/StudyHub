using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StudyHub.Data;
using StudyHub.Hubs;
using StudyHub.Models;
using StudyHub.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddAuthorization();

Env.Load();

var OpenAiKey = Environment.GetEnvironmentVariable("OpenAiKey");

if (string.IsNullOrEmpty(OpenAiKey))
{
    throw new ArgumentNullException(nameof(OpenAiKey));
}

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("production", opt =>
    {
        opt.PermitLimit = 4;
        opt.Window = TimeSpan.FromSeconds(12);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});


builder.Services.AddHttpClient(
    "OpenAIClient",
    client =>
    {
        client.BaseAddress = new Uri("https://api.openai.com/v1/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAiKey);

    });

var environmentProtection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if(environmentProtection is null) throw new Exception("Unable to load environment");

if(environmentProtection == "Development")
{
    var sqliteConnection = "Data Source=app.dev.db";
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

builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 7;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IStudyRoomService, StudyRoomService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IOpenAiService, OpenAiService>();

var app = builder.Build();

app.UseRouting();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRateLimiter();

app.MapIdentityApi<CustomUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<StudyRoomHub>("/chat");

app.Run();
