namespace LibraryAPI.DTOs
{
    public class ReorderBooksDto
    {
        public List<BookOrderDto> Books { get; set; } = new List<BookOrderDto>();
    }

    public class BookOrderDto
    {
        public int BookId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
