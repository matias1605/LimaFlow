using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using LimaFlow.Api.Models;
using LimaFlow.Api.Middlewares; 
using LimaFlow.Api.Validators; // <--- ¡Asegúrate de agregar esta línea!
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Registra los controladores en el sistema
builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la conexión a PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro del manejador global de errores
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); 

// Registrar FluentValidation buscando los validadores del proyecto
builder.Services.AddValidatorsFromAssemblyContaining<IncidenciaValidator>();
    
var app = builder.Build();

// El middleware de excepciones siempre va primero
app.UseExceptionHandler(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Direcciona las peticiones a los controladores
app.MapControllers(); 

app.Run();