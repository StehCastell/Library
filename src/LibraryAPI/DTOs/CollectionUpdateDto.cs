using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class CollectionUpdateDto
    {
        [Required(ErrorMessage = "Collection name is required")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
