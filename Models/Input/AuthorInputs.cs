using System.ComponentModel.DataAnnotations;

namespace GraphQL_Learning.Models.Input
{
    public record class AddAuthorInput
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "El nombre debe tener entre 6 y 100 caracteres.")]
        public string Name { get; init; } = string.Empty;
    }

    public record class UpdateAuthorInput
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor o igual a 1.")]
        public int Id { get; init; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "El nombre debe tener entre 6 y 100 caracteres.")]
        public string Name { get; init; } = string.Empty;
    }
}