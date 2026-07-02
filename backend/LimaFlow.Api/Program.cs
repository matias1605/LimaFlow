using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using LimaFlow.Api.Models;
using LimaFlow.Api.Middlewares; // <--- ¡Asegúrate de agregar esta línea!

var builder = WebApplication.CreateBuilder(args);

// 1. Agrega esto para registrar los controladores en el sistema
builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la conexión a PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails(); // Agrega soporte nativo para detalles de problemas
var app = builder.Build();
app.UseExceptionHandler(); // <--- ¡Esto atrapa los errores en el aire!
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Agrega esto para que el API sepa cómo direccionar las peticiones a los controladores
app.MapControllers(); 

app.Run();