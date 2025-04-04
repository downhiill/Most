using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MostAPI.Context;
using MostAPI.Data;
using MostAPI.IService;
using MostAPI.Service;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // ��������� �������� ������
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

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
builder.Services.AddScoped<MongoDBService>();
builder.Services.AddScoped<IFaqService, FaqService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IServiceService, ServicesService>();

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
