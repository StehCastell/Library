using LibraryAPI.DTOs;

namespace LibraryAPI.Services
{
    public interface ICollectionService
    {
        Task<CollectionResponseDto?> CreateAsync(int userId, CollectionCreateDto collectionDto);
        Task<CollectionResponseDto?> UpdateAsync(int id, int userId, CollectionUpdateDto collectionDto);
        Task<bool> DeleteAsync(int id, int userId);
        Task<CollectionResponseDto?> GetByIdAsync(int id, int userId);
        Task<IEnumerable<CollectionResponseDto>> GetByUserIdAsync(int userId);
        Task<bool> AddBookToCollectionAsync(int collectionId, int bookId, int userId);
        Task<bool> RemoveBookFromCollectionAsync(int collectionId, int bookId, int userId);
        Task<bool> AddAuthorToCollectionAsync(int collectionId, int authorId, int userId);
        Task<bool> RemoveAuthorFromCollectionAsync(int collectionId, int authorId, int userId);
        Task<bool> ReorderBooksAsync(int collectionId, int userId, ReorderBooksDto reorderDto);
    }
}
