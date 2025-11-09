using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetByUserIdAsync(int userId)
        {
            return await _context.Books
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(l => l.Id == id);
        }

        public async Task<bool> AddAuthorToBookAsync(int bookId, int authorId)
        {
            try
            {
                // Verifica se já existe
                var exists = await _context.BookAuthors
                    .AnyAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

                if (exists)
                {
                    Console.WriteLine($"⚠️ Author {authorId} already exists in book {bookId}, returning true (idempotent)");
                    return true;
                }

                var bookAuthor = new BookAuthor
                {
                    BookId = bookId,
                    AuthorId = authorId
                };

                _context.BookAuthors.Add(bookAuthor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in AddAuthorToBookAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId)
        {
            var bookAuthor = await _context.BookAuthors
                .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);

            if (bookAuthor == null)
                return false;

            _context.BookAuthors.Remove(bookAuthor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
