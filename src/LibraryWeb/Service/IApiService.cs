using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public interface IApiService
    {
        // Usuários
        Task<User?> RegisterAsync(RegisterViewModel model);
        Task<User?> LoginAsync(LoginViewModel model);
        Task<User?> GetUserByIdAsync(int id);

        // Books
        Task<List<Book>> GetBooksAsync(int userId);
        Task<Book?> GetBookByIdAsync(int userId, int bookId);
        Task<Book?> CreateBookAsync(int userId, Book book);
        Task<Book?> UpdateBookAsync(int userId, int bookId, Book book);
        Task<bool> DeleteBookAsync(int userId, int bookId);
    }
}
