using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "A senha atual é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
