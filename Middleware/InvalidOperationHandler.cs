using HotChocolate.Resolvers;

namespace GraphQL_Learning.Middleware
{
    public class InvalidOperationHandler: IGraphMiddleware
    {
        private readonly FieldDelegate _next;

        public InvalidOperationHandler(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            try
            {
                await _next(context);
            }
             catch (InvalidOperationException ex) {
                context.ReportError(
                    ErrorBuilder.New()
                    .SetMessage(ex.Message)
                    .SetCode("INVALID OPERATION")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }
    }
}
