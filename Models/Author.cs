using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace GraphQL_Learning.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El campo Name debe tener entre 5 y 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = new List<Book>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Boolean IsActive { get; set; } = true;
    }
}
