using Microsoft.EntityFrameworkCore;
using MostAPI.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


// Добавляем настройку CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Разрешаем доступ с любого источника
              .AllowAnyMethod()    // Разрешаем любые методы HTTP (GET, POST и т.д.)
              .AllowAnyHeader();   // Разрешаем любые заголовки
    });
});

// Настройка Swagger (если она уже есть)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = ConvertPostgresqlUrlToConnectionString(connectionString);
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
string ConvertPostgresqlUrlToConnectionString(string postgresqlUrl)
{
    var uri = new Uri(postgresqlUrl);
    var userInfo = uri.UserInfo.Split(':');

    return $"Host={uri.Host};Port=5432;Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')}";
}
// Устанавливаем порт для запуска приложения на Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем CORS для всех маршрутов
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
