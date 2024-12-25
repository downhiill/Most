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
// ������������� ���� ��� ������� ���������� �� Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
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
