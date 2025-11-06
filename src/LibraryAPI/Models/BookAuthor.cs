using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("book_authors")]
    public class BookAuthor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("book_id")]
        public int BookId { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}
