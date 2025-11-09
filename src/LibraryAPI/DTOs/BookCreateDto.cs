using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class BookCreateDto
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author é obrigatório")]
        [StringLength(100, ErrorMessage = "Author deve ter no máximo 100 caracteres")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gênero é obrigatório")]
        [StringLength(50, ErrorMessage = "Gênero deve ter no máximo 50 caracteres")]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número de páginas é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Número de páginas deve ser maior que zero")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Type é obrigatório")]
        [RegularExpression("^(physical|digital)$", ErrorMessage = "Type deve ser 'fisico' ou 'digital'")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status é obrigatório")]
        [RegularExpression("^(read|reading|not-read|abandoned)$", ErrorMessage = "Status deve ser 'read', 'reading', 'not-read' ou 'abandoned'")]
        public string Status { get; set; } = string.Empty;
    }
}
