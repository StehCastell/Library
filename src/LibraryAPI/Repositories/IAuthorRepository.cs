using LibraryAPI.Models;

namespace LibraryAPI.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(int id);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<IEnumerable<Author>> GetByUserIdAsync(int userId);
        Task<Author> CreateAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
