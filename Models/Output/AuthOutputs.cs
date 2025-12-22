namespace GraphQL_Learning.Models.Output
{
    public record class UserAuthOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public record class CredentialsAuthOutput
    {
        public string Username { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
    }
}
