using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Service;

namespace GraphQL_Learning.Mutation
{
    [ExtendObjectType("Mutation")]
    public class AuthorMutation
    {
        public Task<Author> AddAuthor(AddAuthorInput input, [Service] AuthorService authorService)
        {
            try
            {
                return authorService.AddAuthorAsync(input);
            } catch (Exception ex)
            {
                if(ex.Message != null && ex.Message.Contains("UNIQUE"))
                {
                    throw new GraphQLException(ErrorBuilder
                        .New()
                        .SetMessage("Ya existe un autor con este nombre")
                        .SetExtension("timestamp", DateTime.Now)
                        .Build());
                }
                throw new GraphQLException(ErrorBuilder
                    .New()
                    .SetMessage("Hay ocurrido un error inesperado")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public async Task<Author?> UpdateAuthor(UpdateAuthorInput input, [Service] AuthorService authorService)
        {
            // validar si existe el author
            var _ = await authorService.GetAuthorAsync(input.Id) ?? throw new GraphQLException(ErrorBuilder
                .New()
                .SetMessage("Author Not Found")
                .SetExtension("timestamp", DateTime.Now)
                .Build());

            try
            {
                return await authorService.UpdateAuthorAsync(input);
            }
            catch (Exception ex)
            {
                if (ex.Message != null && ex.Message.Contains("UNIQUE"))
                {
                    throw new GraphQLException(ErrorBuilder
                        .New()
                        .SetMessage("Ya existe un autor con este nombre")
                        .SetExtension("timestamp", DateTime.Now)
                        .Build());
                }
                throw new GraphQLException(ErrorBuilder
                    .New()
                    .SetMessage("Ha ocurrido un error inesperado")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public async Task<bool> DeleteAuthor(int id, [Service] AuthorService service)
            => await service.DeleteAuthorAsync(id);
    }
}
