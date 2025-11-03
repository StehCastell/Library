using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("author")]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("genre")]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Column("pages")]
        public int Pages { get; set; }

        [Required]
        [StringLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty; // "physical" or "digital"

        [Required]
        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; } = string.Empty; // "read", "reading", "unread"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationship with User
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
