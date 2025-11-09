using LibraryAPI.Models;

namespace LibraryAPI.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetByUserIdAsync(int userId);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> AddAuthorToBookAsync(int bookId, int authorId);
        Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId);
    }
}
