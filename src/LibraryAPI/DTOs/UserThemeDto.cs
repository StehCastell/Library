using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class UserThemeDto
    {
        [Required(ErrorMessage = "Theme é obrigatório")]
        [RegularExpression("^(light|dark)$", ErrorMessage = "Theme deve ser 'light' ou 'dark'")]
        public string Theme { get; set; } = string.Empty;
    }
}
