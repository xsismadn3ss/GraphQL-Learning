using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Models.Output;
using GraphQL_Learning.Services;

namespace GraphQL_Learning.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthMutation
    {
        public async Task<CredentialsAuthOutput> Login(
            LoginAuthInput input,
            [Service] AuthService authService)
               => await authService.LoginAsync(input);
    }
}
