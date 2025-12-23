using HotChocolate.Resolvers;

namespace GraphQL_Learning.Middleware
{
    public interface IGraphMiddleware
    {
        Task InvokeAsync(IMiddlewareContext context);
    }
}
