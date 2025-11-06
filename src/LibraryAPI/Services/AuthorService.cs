using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<AuthorResponseDto?> CreateAsync(int userId, AuthorCreateDto authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
                Nationality = authorDto.Nationality,
                Bio = authorDto.Bio,
                CreatedAt = DateTime.Now
            };

            var createdAuthor = await _authorRepository.CreateAsync(author);

            return MapToResponseDto(createdAuthor);
        }

        public async Task<AuthorResponseDto?> UpdateAsync(int id, int userId, AuthorUpdateDto authorDto)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            author.Name = authorDto.Name;
            author.Nationality = authorDto.Nationality;
            author.Bio = authorDto.Bio;

            var updatedAuthor = await _authorRepository.UpdateAsync(author);

            return MapToResponseDto(updatedAuthor);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return false;
            }

            return await _authorRepository.DeleteAsync(id);
        }

        public async Task<AuthorResponseDto?> GetByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            return MapToResponseDto(author);
        }

        public async Task<IEnumerable<AuthorResponseDto>> GetByUserIdAsync(int userId)
        {
            var authors = await _authorRepository.GetByUserIdAsync(userId);

            return authors.Select(MapToResponseDto);
        }

        private AuthorResponseDto MapToResponseDto(Author author)
        {
            return new AuthorResponseDto
            {
                Id = author.Id,
                Name = author.Name,
                Nationality = author.Nationality,
                Bio = author.Bio,
                CreatedAt = author.CreatedAt,
                BookCount = author.BookAuthors?.Count ?? 0
            };
        }
    }
}
