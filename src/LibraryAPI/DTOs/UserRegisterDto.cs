using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Name é obrigatório")]
        [StringLength(100, ErrorMessage = "Name deve ter no máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter entre 6 e 100 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
