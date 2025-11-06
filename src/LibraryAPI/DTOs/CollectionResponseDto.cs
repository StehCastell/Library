namespace LibraryAPI.DTOs
{
    public class CollectionResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookCount { get; set; } // Number of books in this collection
        public List<CollectionBookDto>? Books { get; set; } // Books in this collection
        public List<CollectionAuthorDto>? Authors { get; set; } // Authors in this collection
    }

    public class CollectionBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class CollectionAuthorDto
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Nationality { get; set; }
    }
}
