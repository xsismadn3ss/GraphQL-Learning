using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Models.Input
{
    public record class AddBookInput
    {
        [Required]
        [StringLength(250, MinimumLength = 5)]
        public String Title { get; set; } = string.Empty;
        [Required]
        public DateOnly PublishedOn { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int AuthorId { get; set; }
    }
    public record class UpdateBookInput{
        [Required]
        [Range(1, int.MaxValue)]
        public int Id {  get; set; }
        [StringLength(250, MinimumLength = 5)]
        public String? Title { get; set; } = string.Empty;
        public DateOnly? PublishedOn { get; set; }
        [Range(1, int.MaxValue)]
        public int? AuthorId { get; set; }
    }
}
