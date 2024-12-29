using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MostAPI.Context;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


// Adding a CORS setting
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Allow access from any source
              .AllowAnyMethod()    // Allow any HTTP methods (GET, POST, etc.)
              .AllowAnyHeader();   // Allow any headers
    });
});

// Swagger setup (if you already have one)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = ConvertPostgresqlUrlToConnectionString(connectionString);
}

builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(connectionString));

// MongoDB setup
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGODB_URL");

if (!string.IsNullOrEmpty(mongoConnectionString))
{
    builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoConnectionString));
}

//Convertation To ConnectionString URL
string ConvertPostgresqlUrlToConnectionString(string postgresqlUrl)
{
    var uri = new Uri(postgresqlUrl);
    var userInfo = uri.UserInfo.Split(':');

    return $"Host={uri.Host};Port=5432;Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')}";
}
// Install PORT for launch app on the Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://*:{port}");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for all routes
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
