using GraphQL_Learning.Exceptions;
using HotChocolate.Resolvers;

namespace GraphQL_Learning.Middleware
{
    public class NotFoundHandler
    {
        private readonly FieldDelegate _next;

        public NotFoundHandler(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.ReportError(
                    ErrorBuilder.New()
                    .SetMessage(ex.Message)
                    .SetCode("NOT FOUND")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }
    }
}
