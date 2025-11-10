namespace LibraryWeb.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookCount { get; set; }
    }
}
