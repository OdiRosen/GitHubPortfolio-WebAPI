using GitHubPortfolio.Services;
using GitHubPortfolio.Services.Models;

var builder = WebApplication.CreateBuilder(args);

// הגדרת ה-Options Pattern לשליפת נתונים רגישים מה-User Secrets
builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHubSettings"));

// רישום שירות ה-In-Memory Cache 
builder.Services.AddMemoryCache();

// רישום השירות ב-Dependency Injection Container
builder.Services.AddScoped<IGitHubService, GitHubService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // מאפשר בדיקה נוחה של ה-API
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();