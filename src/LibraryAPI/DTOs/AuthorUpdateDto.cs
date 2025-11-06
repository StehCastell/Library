using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class AuthorUpdateDto
    {
        [Required(ErrorMessage = "Author name is required")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Nationality cannot exceed 100 characters")]
        public string? Nationality { get; set; }

        public string? Bio { get; set; }
    }
}
