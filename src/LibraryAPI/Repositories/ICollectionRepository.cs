using LibraryAPI.Models;

namespace LibraryAPI.Repositories
{
    public interface ICollectionRepository
    {
        Task<Collection?> GetByIdAsync(int id);
        Task<IEnumerable<Collection>> GetByUserIdAsync(int userId);
        Task<Collection> CreateAsync(Collection collection);
        Task<Collection> UpdateAsync(Collection collection);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> AddBookToCollectionAsync(int collectionId, int bookId, int displayOrder);
        Task<bool> RemoveBookFromCollectionAsync(int collectionId, int bookId);
        Task<bool> AddAuthorToCollectionAsync(int collectionId, int authorId);
        Task<bool> RemoveAuthorFromCollectionAsync(int collectionId, int authorId);
        Task<bool> ReorderBooksAsync(int collectionId, Dictionary<int, int> bookOrders);
    }
}
