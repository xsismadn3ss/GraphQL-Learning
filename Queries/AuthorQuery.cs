using GraphQL_Learning.Models;
using GraphQL_Learning.Service;

namespace GraphQL_Learning.Queries
{
    [ExtendObjectType("Query")]
    public class AuthorQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Author> GetAuthors([Service] AuthorService authorService)
            => authorService.GetAuthors();

        public async Task<Author> GetAuthor(int id, [Service]  AuthorService authorService)
            => await authorService.GetAuthorAsync(id) ?? throw new GraphQLException(ErrorBuilder
                .New()
                .SetMessage("Author not found")
                .SetExtension("timestamp", DateTime.Now)
                .Build());
    }
}
