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

var app = builder.Build();

// Устанавливаем порт для запуска приложения на Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Render задает переменную PORT
app.Urls.Add($"http://*:{port}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем CORS для всех маршрутов
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
