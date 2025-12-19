using GraphQL_Learning.Exceptions;
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
            }
            catch (DuplicateEntityException duplicateEx)
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(duplicateEx.Message)
                    .SetCode("UNIQUE_CONSTRAINT_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public async Task<Author?> UpdateAuthor(UpdateAuthorInput input, [Service] AuthorService authorService)
        {
            try
            {
                return await authorService.UpdateAuthorAsync(input);
            }
            catch (DuplicateEntityException duplicateEx)
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(duplicateEx.Message)
                    .SetCode("UNIQUE_CONSTRAINT_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
            catch (NotFoundException notFoundEx)
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(notFoundEx.Message)
                    .SetCode("AUTHOR_NOT_FOUND_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public async Task<bool> DeleteAuthor(int id, [Service] AuthorService service)
            => await service.DeleteAuthorAsync(id);
    }
}
