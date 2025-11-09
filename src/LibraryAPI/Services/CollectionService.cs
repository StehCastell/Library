using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;

        public CollectionService(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        public async Task<CollectionResponseDto?> CreateAsync(int userId, CollectionCreateDto collectionDto)
        {
            var collection = new Collection
            {
                UserId = userId,
                Name = collectionDto.Name,
                Description = collectionDto.Description,
                CreatedAt = DateTime.Now
            };

            var createdCollection = await _collectionRepository.CreateAsync(collection);

            return MapToResponseDto(createdCollection);
        }

        public async Task<CollectionResponseDto?> UpdateAsync(int id, int userId, CollectionUpdateDto collectionDto)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);

            if (collection == null || collection.UserId != userId)
            {
                return null;
            }

            collection.Name = collectionDto.Name;
            collection.Description = collectionDto.Description;

            var updatedCollection = await _collectionRepository.UpdateAsync(collection);

            return MapToResponseDto(updatedCollection);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);

            if (collection == null || collection.UserId != userId)
            {
                return false;
            }

            return await _collectionRepository.DeleteAsync(id);
        }

        public async Task<CollectionResponseDto?> GetByIdAsync(int id, int userId)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);

            if (collection == null || collection.UserId != userId)
            {
                return null;
            }

            return MapToResponseDto(collection);
        }

        public async Task<IEnumerable<CollectionResponseDto>> GetByUserIdAsync(int userId)
        {
            var collections = await _collectionRepository.GetByUserIdAsync(userId);

            return collections.Select(MapToResponseDto);
        }

        public async Task<bool> AddBookToCollectionAsync(int collectionId, int bookId, int userId)
        {
            Console.WriteLine($"🔍 CollectionService.AddBookToCollectionAsync - collectionId: {collectionId}, bookId: {bookId}, userId: {userId}");

            var collection = await _collectionRepository.GetByIdAsync(collectionId);

            if (collection == null)
            {
                Console.WriteLine($"❌ Collection {collectionId} not found");
                return false;
            }

            if (collection.UserId != userId)
            {
                Console.WriteLine($"❌ Collection {collectionId} does not belong to user {userId}. Collection.UserId: {collection.UserId}");
                return false;
            }

            Console.WriteLine($"✅ Collection validated. Current book count: {collection.CollectionBooks?.Count ?? 0}");

            // Calculate next display order
            var maxOrder = collection.CollectionBooks?.Select(cb => cb.DisplayOrder).DefaultIfEmpty(0).Max() ?? 0;
            Console.WriteLine($"📊 Next display order: {maxOrder + 1}");

            var result = await _collectionRepository.AddBookToCollectionAsync(collectionId, bookId, maxOrder + 1);
            Console.WriteLine($"🎯 Repository returned: {result}");

            return result;
        }

        public async Task<bool> RemoveBookFromCollectionAsync(int collectionId, int bookId, int userId)
        {
            var collection = await _collectionRepository.GetByIdAsync(collectionId);

            if (collection == null || collection.UserId != userId)
            {
                return false;
            }

            return await _collectionRepository.RemoveBookFromCollectionAsync(collectionId, bookId);
        }

        public async Task<bool> AddAuthorToCollectionAsync(int collectionId, int authorId, int userId)
        {
            var collection = await _collectionRepository.GetByIdAsync(collectionId);

            if (collection == null || collection.UserId != userId)
            {
                return false;
            }

            return await _collectionRepository.AddAuthorToCollectionAsync(collectionId, authorId);
        }

        public async Task<bool> RemoveAuthorFromCollectionAsync(int collectionId, int authorId, int userId)
        {
            var collection = await _collectionRepository.GetByIdAsync(collectionId);

            if (collection == null || collection.UserId != userId)
            {
                return false;
            }

            return await _collectionRepository.RemoveAuthorFromCollectionAsync(collectionId, authorId);
        }

        private CollectionResponseDto MapToResponseDto(Collection collection)
        {
            return new CollectionResponseDto
            {
                Id = collection.Id,
                UserId = collection.UserId,
                Name = collection.Name,
                Description = collection.Description,
                CreatedAt = collection.CreatedAt,
                BookCount = collection.CollectionBooks?.Count ?? 0,
                Books = collection.CollectionBooks?
                    .OrderBy(cb => cb.DisplayOrder)
                    .Select(cb => new CollectionBookDto
                    {
                        BookId = cb.BookId,
                        Id = cb.Book?.Id ?? 0,
                        Title = cb.Book?.Title ?? "",
                        Author = cb.Book?.Author ?? "",
                        Status = cb.Book?.Status ?? "",
                        DisplayOrder = cb.DisplayOrder
                    }).ToList(),
                Authors = collection.CollectionAuthors?
                    .Select(ca => new CollectionAuthorDto
                    {
                        AuthorId = ca.AuthorId,
                        Name = ca.Author?.Name ?? "",
                        Nationality = ca.Author?.Nationality
                    })
                    .OrderBy(a => a.Name)
                    .ToList()
            };
        }
    }
}
