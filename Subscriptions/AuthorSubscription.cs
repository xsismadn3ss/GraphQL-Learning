using GraphQL_Learning.Models;

namespace GraphQL_Learning.Subscriptions
{
    [ExtendObjectType("Subscription")]
    public class AuthorSubscription
    {
        [Subscribe]
        [Topic("onAuthorAdded")]
        public Author OnAuthorAdded([EventMessage] Author author)
            => author;

        [Subscribe]
        [Topic("onAuthorUpdated")]
        public Author OnAuthorUpdated([EventMessage] Author author)
            => author;
    }
}
