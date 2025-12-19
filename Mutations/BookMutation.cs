using GraphQL_Learning.Exceptions;
using GraphQL_Learning.Models;
using GraphQL_Learning.Models.Input;
using GraphQL_Learning.Service;

namespace GraphQL_Learning.Mutation
{
    [ExtendObjectType("Mutation")]
    public class BookMutation
    {
        public async Task<Book> AddBook(AddBookInput input, [Service] BookService bookService, [Service] AuthorService authorService)
        {
            var _ = await authorService.GetAuthorAsync(input.AuthorId) ?? throw new GraphQLException(ErrorBuilder
                    .New()
                    .SetMessage("Author not found")
                    .SetCode("AUTHOR_NOT_FOUND_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            try
            { 
                return await bookService.AddBookAsync(input);
            }
            catch(DuplicateEntityException duplicateEx)
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(duplicateEx.Message)
                    .SetCode("UNIQUE_CONSTRAINT_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public async Task<Book?> UpdateBook(UpdateBookInput input, [Service] BookService bookService, [Service] AuthorService authorService)
        {
            if (input.AuthorId.HasValue)
            {
                var _ = await authorService.GetAuthorAsync(input.AuthorId.Value) ?? throw new GraphQLException(ErrorBuilder
                    .New()
                    .SetMessage("Author not found")
                    .SetCode("AUTHOR_NOT_FOUND_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
            try
            {
                return await bookService.UpdateBookAsync(input);
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
                    .SetCode("BOOK_NOT_FOUND_ERROR")
                    .SetExtension("timestamp", DateTime.Now)
                    .Build());
            }
        }

        public Task<bool> DeleteBook(int id, [Service] BookService bookService)
            => bookService.DeleteBookAsync(id);
    }
}
