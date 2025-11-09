using LibraryAPI.DTOs;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        Task<BookResponseDto?> CreateAsync(int userId, BookCreateDto bookDto);
        Task<BookResponseDto?> UpdateAsync(int id, int userId, BookUpdateDto bookDto);
        Task<bool> DeleteAsync(int id, int userId);
        Task<BookResponseDto?> GetByIdAsync(int id, int userId);
        Task<IEnumerable<BookResponseDto>> GetByUserIdAsync(int userId);
        Task<bool> AddAuthorToBookAsync(int bookId, int authorId, int userId);
        Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId, int userId);
        Task<IEnumerable<AuthorResponseDto>?> GetBookAuthorsAsync(int bookId, int userId);
    }
}
