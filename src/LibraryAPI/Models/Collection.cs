using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("collections")]
    public class Collection
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "TEXT")]
        public string? Description { get; set; }

        [Column("profile_image", TypeName = "LONGTEXT")]
        public string? ProfileImage { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual List<CollectionBook>? CollectionBooks { get; set; }
        public virtual List<CollectionAuthor>? CollectionAuthors { get; set; }
    }
}
