using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [StringLength(50, ErrorMessage = "Email must be less than 50 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be greater than 6 characters and less than 50 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
