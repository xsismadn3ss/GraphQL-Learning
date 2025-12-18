using GraphQL_Learning.Models;
using GraphQL_Learning.Service;

namespace GraphQL_Learning.Queries
{
    [ExtendObjectType("Query")]
    public class BookQuery
    {
        [UsePaging]
        [UseSorting]
        [UseFiltering]
        [UseProjection]
        public IQueryable<Book> GetBooks([Service] BookService bookService)
            => bookService.GetBooks();

        public async Task<Book> GetBook(int id, [Service] BookService bookService)
            => await bookService.GetBookAsync(id) ?? throw new GraphQLException(ErrorBuilder
                .New()
                .SetMessage("Book Not Found")
                .SetExtension("timestamp", DateTime.Now)
                .Build());
    }
}
