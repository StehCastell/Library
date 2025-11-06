using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("collection_books")]
    public class CollectionBook
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("collection_id")]
        public int CollectionId { get; set; }

        [Required]
        [Column("book_id")]
        public int BookId { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        [ForeignKey("CollectionId")]
        public virtual Collection? Collection { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }
    }
}
