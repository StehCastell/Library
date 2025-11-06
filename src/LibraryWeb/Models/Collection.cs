namespace LibraryWeb.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookCount { get; set; }
        public List<CollectionBook>? Books { get; set; }
    }

    public class CollectionBook
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
