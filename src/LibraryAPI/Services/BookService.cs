using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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
                CreatedAt = book.CreatedAt
            };
        }
    }
}
