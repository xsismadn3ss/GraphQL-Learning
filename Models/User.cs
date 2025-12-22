using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Boolean IsActive { get; set; } = true;
    }
}
