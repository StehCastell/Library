using LibraryAPI.DTOs;

namespace LibraryAPI.Services
{
    public interface IAuthorService
    {
        Task<AuthorResponseDto?> CreateAsync(int userId, AuthorCreateDto authorDto);
        Task<AuthorResponseDto?> UpdateAsync(int id, int userId, AuthorUpdateDto authorDto);
        Task<bool> DeleteAsync(int id, int userId);
        Task<AuthorResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<AuthorResponseDto>> GetByUserIdAsync(int userId);
    }
}
