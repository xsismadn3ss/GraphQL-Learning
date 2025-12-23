using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Models.Output;
using GraphQL_Learning.Services;

namespace GraphQL_Learning.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthMutation
    {
        private static void SetTokenCookie(IHttpContextAccessor httpContext, string token)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            };
            httpContext.HttpContext?.Response.Cookies.Append("auth-token", token, cookieOptions);
        }
        public async Task<CredentialsAuthOutput> Login(
            LoginAuthInput input,
            [Service] AuthService authService,
            [Service] IHttpContextAccessor httpContext)
        {

            var credentials = await authService.LoginAsync(input);
            SetTokenCookie(httpContext, credentials.AuthToken);
            return credentials;
        }

        public async Task<CredentialsAuthOutput> Register(
            RegisterAuthInput input,
            [Service] AuthService authService,
            [Service] IHttpContextAccessor httpContext)
        {

            var credentials = await authService.RegisterAsync(input);
            SetTokenCookie(httpContext, credentials.AuthToken);
            return credentials;
        }
    }
}
