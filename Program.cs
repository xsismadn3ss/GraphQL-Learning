using GraphQL_Learning.Middleware;
using GraphQL_Learning.Models;
using GraphQL_Learning.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar secretos de usuario (dotnet user-secrets)
builder.Configuration.AddUserSecrets<Program>();

var isDevelopmnet = builder.Environment.IsDevelopment();

// Obtener cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// #### Agregar servicios al contenedor. ####

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("auth-token"))
                {
                    context.Token = context.Request.Cookies["auth-token"];
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// Obtener tipos GraphQL dinamicamente
Type[] types = [..TypesMapper.GetQueriesTypes()
    .Union(TypesMapper.GetMutationsTypes()
    .Union(TypesMapper.GetSubscriptionsTypes()))];


// #### Configurar GraphQL ####
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
     // middleware
    .UseField<ValidationMiddleware>()
    .UseField<NotFoundHandler>()
    .UseField<DuplicateEntityHandler>()
    .UseField<InvalidCredentialsHandler>()
    .UseField<InvalidOperationHandler>()
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
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL().WithOptions(new()
{
    Tool = { Enable = isDevelopmnet }, // habilitar Nitro solo en desarrollo
    EnableGetRequests = isDevelopmnet, // Permitir hacer peticiones desde herramientas externas solo en desarrollo
    EnableSchemaRequests = isDevelopmnet // Permitir obtener esquema solo en desarrollo
});

app.Run();
