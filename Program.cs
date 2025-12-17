using GraphQL_Learning.Models;
using GraphQL_Learning.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar secretos de usuario (dotnet user-secrets)
builder.Configuration.AddUserSecrets<Program>();

// Obtener cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar servicios al contenedor.

// registrar servicios dinamicamente
foreach (var type in TypesMapper.GetServiceTypes())
{
    builder.Services.AddScoped(type);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);


var app = builder.Build();

app.Run();
