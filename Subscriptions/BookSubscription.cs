using GraphQL_Learning.Models;
using HotChocolate.Authorization;

namespace GraphQL_Learning.Subscriptions
{
    [Authorize(Roles = new[] {"ADMIN", "SUPERVISOR"})]
    [ExtendObjectType("Subscription")]
    public class BookSubscription
    {
        [Subscribe]
        [Topic("onBookAdded")]
        public Book OnBookAdded([EventMessage] Book book)
            => book;

        [Subscribe]
        [Topic("onBookUpdated")]
        public Book OnBookUpdated([EventMessage] Book book)
            => book;

        [Subscribe]
        [Topic("onBookAdded_{authorId}")]
        public Book OnBookCreatedByAUthor(int authorId, [EventMessage] Book book)
            => book;

        [Subscribe]
        [Topic("onBookUpdated_{authorId}")]
        public Book OnBookUpdatedByAUthor(int authorId, [EventMessage] Book book)
            => book;
    }
}
