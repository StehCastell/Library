using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<BookResponseDto?> CreateAsync(int userId, BookCreateDto bookDto)
        {
            var book = new Book
            {
                UserId = userId,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                Pages = bookDto.Pages,
                Type = bookDto.Type,
                Status = bookDto.Status,
                CoverImage = bookDto.CoverImage,
                CreatedAt = DateTime.Now
            };

            var bookCriado = await _bookRepository.CreateAsync(book);

            return MapToResponseDto(bookCriado);
        }

        public async Task<BookResponseDto?> UpdateAsync(int id, int userId, BookUpdateDto bookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null || book.UserId != userId)
            {
                return null;
            }

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Genre = bookDto.Genre;
            book.Pages = bookDto.Pages;
            book.Type = bookDto.Type;
            book.Status = bookDto.Status;
            book.CoverImage = bookDto.CoverImage;

            var bookAtualizado = await _bookRepository.UpdateAsync(book);

            return MapToResponseDto(bookAtualizado);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null || book.UserId != userId)
            {
                return false;
            }

            return await _bookRepository.DeleteAsync(id);
        }

        public async Task<BookResponseDto?> GetByIdAsync(int id, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null || book.UserId != userId)
            {
                return null;
            }

            return MapToResponseDto(book);
        }

        public async Task<IEnumerable<BookResponseDto>> GetByUserIdAsync(int userId)
        {
            var books = await _bookRepository.GetByUserIdAsync(userId);

            return books.Select(MapToResponseDto);
        }

        public async Task<bool> AddAuthorToBookAsync(int bookId, int authorId, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book == null || book.UserId != userId)
            {
                return false;
            }

            return await _bookRepository.AddAuthorToBookAsync(bookId, authorId);
        }

        public async Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book == null || book.UserId != userId)
            {
                return false;
            }

            return await _bookRepository.RemoveAuthorFromBookAsync(bookId, authorId);
        }

        public async Task<IEnumerable<AuthorResponseDto>?> GetBookAuthorsAsync(int bookId, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book == null || book.UserId != userId)
            {
                return null;
            }

            var authors = await _authorRepository.GetAuthorsByBookIdAsync(bookId);

            return authors.Select(a => new AuthorResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Nationality = a.Nationality,
                Bio = a.Bio
            });
        }

        private BookResponseDto MapToResponseDto(Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                UserId = book.UserId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Pages = book.Pages,
                Type = book.Type,
                Status = book.Status,
                CoverImage = book.CoverImage,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
