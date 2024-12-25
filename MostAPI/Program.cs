using Microsoft.EntityFrameworkCore;
using MostAPI.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


// ��������� ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // ��������� ������ � ������ ���������
              .AllowAnyMethod()    // ��������� ����� ������ HTTP (GET, POST � �.�.)
              .AllowAnyHeader();   // ��������� ����� ���������
    });
});

// ��������� Swagger (���� ��� ��� ����)
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
// ������������� ���� ��� ������� ���������� �� Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �������� CORS ��� ���� ���������
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
