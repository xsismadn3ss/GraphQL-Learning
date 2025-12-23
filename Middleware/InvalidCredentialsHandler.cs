using HotChocolate.Resolvers;
using System.Security.Authentication;

namespace GraphQL_Learning.Middleware
{
    public class InvalidCredentialsHandler: IGraphMiddleware
    {
        private readonly FieldDelegate _next;

        public InvalidCredentialsHandler(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidCredentialException ex)
            {
                context.ReportError(
                    ErrorBuilder.New()
                    .SetMessage(ex.Message)
                    .SetCode("INVALID CREDENTIALS")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }
    }
}
