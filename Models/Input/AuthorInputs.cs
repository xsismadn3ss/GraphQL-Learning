namespace GraphQL_Learning.Models.Input
{
    public record AddAuthorInput(String Name);
    public record UpdateAuthorInput(int id, String Name);
}
