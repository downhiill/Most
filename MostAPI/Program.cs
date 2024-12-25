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

var app = builder.Build();

// ������������� ���� ��� ������� ���������� �� Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Render ������ ���������� PORT
app.Urls.Add($"http://*:{port}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �������� CORS ��� ���� ���������
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
