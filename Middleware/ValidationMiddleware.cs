using HotChocolate.Resolvers;
using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Middleware
{
    public class ValidationMiddleware: IGraphMiddleware
    {
        private readonly FieldDelegate _next;

        public ValidationMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            foreach (var arg in context.Selection.Field.Arguments)
            {
                var value = context.ArgumentValue<object>(arg.Name);
                if (value != null)
                {
                    var ctx = new ValidationContext(value);
                    var results = new List<ValidationResult>();

                    if (!Validator.TryValidateObject(value, ctx, results, true))
                    {
                        var errorBuilder = ErrorBuilder.New().
                            SetMessage("Validation failed")
                            .SetExtension("code", "VALIDATION_ERROR");
                        
                        foreach (var r in results)
                        {
                            var field = string.Join(",", r.MemberNames);
                            var message = r.ErrorMessage ?? "Unkown validation error";

                            errorBuilder.SetExtension(field, message);
                        }
                        throw new GraphQLException(errorBuilder.Build());
                    }
                }
            }
            await _next(context);
        }
    }
}
