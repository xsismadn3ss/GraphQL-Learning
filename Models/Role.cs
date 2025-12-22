using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Boolean IsActive { get; set; } = true;
    }
}
