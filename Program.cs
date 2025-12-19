using GraphQL_Learning.Models;
using GraphQL_Learning.Service;
using Microsoft.EntityFrameworkCore;
using GraphQL_Learning.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Agregar secretos de usuario (dotnet user-secrets)
builder.Configuration.AddUserSecrets<Program>();

var isDevelopmnet = builder.Environment.IsDevelopment();

// Obtener cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar servicios al contenedor.

// registrar servicios dinamicamente
Console.WriteLine("\nServicios registrados:");
foreach (var type in TypesMapper.GetServiceTypes())
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(type.Name);
    Console.ForegroundColor = ConsoleColor.White;
    builder.Services.AddScoped(type);
}
Console.WriteLine();

// Configurar Db Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);


// Obtener tipos GraphQL
Type[] types = [..TypesMapper.GetQueriesTypes()
    .Union(TypesMapper.GetMutationsTypes()
    .Union(TypesMapper.GetSubscriptionsTypes()))];

Console.WriteLine("Tipos GraphQL indentificados:");
foreach (var t in types)
{
    Console.ForegroundColor= ConsoleColor.Green;
    Console.WriteLine(t.Name); 
    Console.ForegroundColor = ConsoleColor.White;
}
Console.WriteLine();


// Configurar GraphQL
builder.Services
    .AddGraphQLServer()
    .UseField<ValidationMiddleware>()
    .AddQueryType(q => q.Name("Query"))
    .AddMutationType(m => m.Name("Mutation"))
    .AddTypes(types)
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = isDevelopmnet);

var app = builder.Build();

app.MapGraphQL().WithOptions(new()
{
    Tool = { Enable = isDevelopmnet }, // habilitar Nitro solo en desarrollo
    EnableGetRequests = isDevelopmnet, // Permitir hacer peticiones desde herramientas externas solo en desarrollo
    EnableSchemaRequests = isDevelopmnet // Permitir obtener esquema solo en desarrollo
});

app.Run();
