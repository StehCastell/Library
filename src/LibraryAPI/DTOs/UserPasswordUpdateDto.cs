using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class UserPasswordUpdateDto
    {
        [Required(ErrorMessage = "A senha atual é obrigatória")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
