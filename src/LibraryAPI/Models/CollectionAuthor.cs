using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    [Table("collection_authors")]
    public class CollectionAuthor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("collection_id")]
        public int CollectionId { get; set; }

        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }

        // Navigation properties
        [ForeignKey("CollectionId")]
        public virtual Collection? Collection { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}
