using Microsoft.EntityFrameworkCore;
using MostAPI;
using MostAPI.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
string databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new InvalidOperationException("DATABASE_URL not set");
builder.Configuration["ConnectionStrings:DefaultConnection"] = DatabaseHelper.ConvertPostgresUrlToConnectionString(databaseUrl);

// ��������� �������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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
