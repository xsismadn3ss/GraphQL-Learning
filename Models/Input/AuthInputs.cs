using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Models.Input
{
    public record class LoginAuthInput
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public record class RegisterAuthInput
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(255, MinimumLength = 8)]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }

    public record class UpdateAuthInput
    {
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(255, MinimumLength = 8)]
        [PasswordPropertyText]
        public string? Password { get; set; }
    }
}
