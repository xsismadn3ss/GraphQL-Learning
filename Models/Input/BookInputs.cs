namespace GraphQL_Learning.Models.Input
{
    public record AddBookInput(
        String Title,
        DateOnly PublishedOn,
        int AuthorId);
    public record UpdateBookInput(
        int? Id,
        String? Title,
        DateOnly? PublishedOn,
        int? AuthorId);
}
