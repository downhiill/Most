using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen(options =>
{
    // Добавляем фильтр для работы с IFormFile
    options.OperationFilter<Swashbuckle.AspNetCore.Filters.FileUploadOperationFilter>();

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Documentation",
        Version = "v1"
    });
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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty; // Swagger будет доступен по корневому адресу
    });
}

// Enable CORS for all routes
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
