using GraphQL_Learning.Exceptions;
using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Models.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_Learning.Services
{
    public class AuthService
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthService(JwtService jwtService, AppDbContext context, IPasswordHasher<string> passwordHasher)
        {
            _jwtService = jwtService;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<CredentialsAuthOutput> LoginAsync(LoginAuthInput input)
        {
            User? user = null;
            if(!string.IsNullOrEmpty(input.Username) && string.IsNullOrEmpty(input.Email))
            {
                user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == input.Username);
            }
            else if (!string.IsNullOrEmpty(input.Email) && string.IsNullOrEmpty(input.Username))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
            }
            else
            {
                throw new InvalidOperationException("No ex permitido ingresar email y username al mismo tiempo. Inicia sesión solo con email o username");
            }
            if (user == null) throw new NotFoundException("User not found");

            // validar password
            var result = _passwordHasher.VerifyHashedPassword(null, user.Password, input.Password);
            // TODO: crear excepcion para credenciales incorrectas
            if (result == PasswordVerificationResult.Failed) throw new InvalidOperationException("Contraseña incorrecta");

            // generar token
            var token = _jwtService.GenerateToken(user.Username, user.Role.Name);
            return new CredentialsAuthOutput
            {
                Username = user.Username,
                AuthToken = token
            };
        }

        public async Task<CredentialsAuthOutput> RegisterAsync(RegisterAuthInput input)
        {
            // validar si ya existe un usuario con mismo email o username
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == input.Username || u.Email == u.Email);
            if(existingUser?.Username == input.Username) throw new DuplicateEntityException("El nombre de usuario ya esta ocupado");
            if (existingUser?.Email == input.Email) throw new DuplicateEntityException("El email ya esta ocupado");

            // guardar usuario
            User user = new()
            {
                Name = input.Username,
                Email = input.Email,
                Password = _passwordHasher.HashPassword(null, input.Password),
                RoleId = 1
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user.Username, user.Role.Name);
            return new CredentialsAuthOutput
            {
                Username = user.Username,
                AuthToken = token
            };
        }
    }
}
