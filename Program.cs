using GraphQL_Learning.Models;
using GraphQL_Learning.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar secretos de usuario (dotnet user-secrets)
builder.Configuration.AddUserSecrets<Program>();

var isDevelopmnet = builder.Environment.IsDevelopment();

// Obtener cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar servicios al contenedor.

// registrar servicios dinamicamente
foreach (var type in TypesMapper.GetServiceTypes())
{
    builder.Services.AddScoped(type);
}

// Configurar Db Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);

// Obtener tipos GraphQL
Type[] types = [..TypesMapper.GetQueriesTypes()
    .Union(TypesMapper.GetMutationsTypes()
    .Union(TypesMapper.GetSubscriptionsTypes()))];

// Configurar GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType(q => q.Name("query"))
    .AddMutationType(m => m.Name("query"))
    .AddTypes(types)
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = isDevelopmnet);

var app = builder.Build();

app.MapGraphQL().WithOptions(new()
{
    Tool = { Enable = isDevelopmnet },
    EnableGetRequests = true,
    EnableSchemaRequests = true
});

app.Run();
