using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly LibraryContext _context;

        public CollectionRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Collection?> GetByIdAsync(int id)
        {
            return await _context.Collections
                .Include(c => c.CollectionBooks)
                .ThenInclude(cb => cb.Book)
                .Include(c => c.CollectionAuthors)
                .ThenInclude(ca => ca.Author)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Collection>> GetByUserIdAsync(int userId)
        {
            return await _context.Collections
                .Include(c => c.CollectionBooks)
                .ThenInclude(cb => cb.Book)
                .Include(c => c.CollectionAuthors)
                .ThenInclude(ca => ca.Author)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Collection> CreateAsync(Collection collection)
        {
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return collection;
        }

        public async Task<Collection> UpdateAsync(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
            return collection;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
                return false;

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Collections.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> AddBookToCollectionAsync(int collectionId, int bookId, int displayOrder)
        {
            var collectionBook = new CollectionBook
            {
                CollectionId = collectionId,
                BookId = bookId,
                DisplayOrder = displayOrder
            };

            _context.CollectionBooks.Add(collectionBook);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveBookFromCollectionAsync(int collectionId, int bookId)
        {
            var collectionBook = await _context.CollectionBooks
                .FirstOrDefaultAsync(cb => cb.CollectionId == collectionId && cb.BookId == bookId);

            if (collectionBook == null)
                return false;

            _context.CollectionBooks.Remove(collectionBook);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAuthorToCollectionAsync(int collectionId, int authorId)
        {
            // Verifica se já existe
            var exists = await _context.CollectionAuthors
                .AnyAsync(ca => ca.CollectionId == collectionId && ca.AuthorId == authorId);

            if (exists)
                return false;

            var collectionAuthor = new CollectionAuthor
            {
                CollectionId = collectionId,
                AuthorId = authorId
            };

            _context.CollectionAuthors.Add(collectionAuthor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAuthorFromCollectionAsync(int collectionId, int authorId)
        {
            var collectionAuthor = await _context.CollectionAuthors
                .FirstOrDefaultAsync(ca => ca.CollectionId == collectionId && ca.AuthorId == authorId);

            if (collectionAuthor == null)
                return false;

            _context.CollectionAuthors.Remove(collectionAuthor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
