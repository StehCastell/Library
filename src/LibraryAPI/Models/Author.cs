using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("authors")]
    public class Author
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("nationality")]
        public string? Nationality { get; set; }

        [Column("bio", TypeName = "TEXT")]
        public string? Bio { get; set; }

        [Column("profile_image", TypeName = "LONGTEXT")]
        public string? ProfileImage { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual List<BookAuthor>? BookAuthors { get; set; }
        public virtual List<CollectionAuthor>? CollectionAuthors { get; set; }
    }
}
