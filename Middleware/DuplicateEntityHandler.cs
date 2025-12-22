using GraphQL_Learning.Exceptions;
using HotChocolate.Resolvers;

namespace GraphQL_Learning.Middleware
{
    public class DuplicateEntityHandler
    {
        private readonly FieldDelegate _next;

        public DuplicateEntityHandler(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DuplicateEntityException ex)
            {
                context.ReportError(
                    ErrorBuilder.New()
                    .SetMessage(ex.Message)
                    .SetCode("DUPLICATE ENTITY")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }
    }
}
