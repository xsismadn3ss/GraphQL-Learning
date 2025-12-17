using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQL_Learning.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Title es requerido")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "El campo TItle no debe tener entre 5 y 250 caracteres.")]
        public string Title { get; set; } = string.Empty;
        public DateOnly PublishedOn { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Boolean IsActive { get; set; } = true;
    }
}
