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

        // Authors
        Task<List<Author>> GetAuthorsAsync(int userId);
        Task<Author?> GetAuthorByIdAsync(int userId, int authorId);
        Task<Author?> CreateAuthorAsync(int userId, Author author);
        Task<Author?> UpdateAuthorAsync(int userId, int authorId, Author author);
        Task<bool> DeleteAuthorAsync(int userId, int authorId);

        // Collections
        Task<List<Collection>> GetCollectionsAsync(int userId);
        Task<Collection?> GetCollectionByIdAsync(int userId, int collectionId);
        Task<Collection?> CreateCollectionAsync(int userId, Collection collection);
        Task<Collection?> UpdateCollectionAsync(int userId, int collectionId, Collection collection);
        Task<bool> DeleteCollectionAsync(int userId, int collectionId);
        Task<bool> AddBookToCollectionAsync(int userId, int collectionId, int bookId);
        Task<bool> RemoveBookFromCollectionAsync(int userId, int collectionId, int bookId);
    }
}
