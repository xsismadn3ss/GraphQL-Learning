using GraphQL_Learning.Models;
using GraphQL_Learning.Services;
using Microsoft.EntityFrameworkCore;
using GraphQL_Learning.Middleware;
using Microsoft.AspNetCore.Identity;

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
builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

// Configurar Db Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);


// Obtener tipos GraphQL dinamicamente
Type[] types = [..TypesMapper.GetQueriesTypes()
    .Union(TypesMapper.GetMutationsTypes()
    .Union(TypesMapper.GetSubscriptionsTypes()))];


// Configurar GraphQL
builder.Services
    .AddGraphQLServer()
     // middleware
    .UseField<ValidationMiddleware>()
    .UseField<NotFoundHandler>()
    .UseField<DuplicateEntityHandler>()
    .UseField<InvalidCredentialsHandler>()
    // tipos
    .AddQueryType(q => q.Name("Query"))
    .AddMutationType(m => m.Name("Mutation"))
    .AddSubscriptionType(s => s.Name("Subscription"))
    .AddInMemorySubscriptions()
    .AddTypes(types)
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = isDevelopmnet);

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL().WithOptions(new()
{
    Tool = { Enable = isDevelopmnet }, // habilitar Nitro solo en desarrollo
    EnableGetRequests = isDevelopmnet, // Permitir hacer peticiones desde herramientas externas solo en desarrollo
    EnableSchemaRequests = isDevelopmnet // Permitir obtener esquema solo en desarrollo
});

app.Run();
