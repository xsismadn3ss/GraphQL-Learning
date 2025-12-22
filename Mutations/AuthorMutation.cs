using GraphQL_Learning.Exceptions;
using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Services;
using HotChocolate.Subscriptions;

namespace GraphQL_Learning.Mutation
{
    [ExtendObjectType("Mutation")]
    public class AuthorMutation
    {
        public async Task<Author> AddAuthor(
            AddAuthorInput input, 
            [Service] AuthorService authorService,
            [Service] ITopicEventSender eventSender)
        {
            try
            {
                var newAuthor = await authorService.AddAuthorAsync(input);
                await eventSender.SendAsync("onAuthorAdded", newAuthor);
                return newAuthor;
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

        public async Task<Author?> UpdateAuthor(
            UpdateAuthorInput input, 
            [Service] AuthorService authorService,
            [Service] ITopicEventSender eventSender)
        {
            try
            {
                var author = await authorService.UpdateAuthorAsync(input);
                await eventSender.SendAsync("onAuthorUpdated", author);
                return author;
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
