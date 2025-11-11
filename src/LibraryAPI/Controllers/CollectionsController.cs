using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        /// <summary>
        /// List all collections for a user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionResponseDto>>> GetAll(int userId)
        {
            var collections = await _collectionService.GetByUserIdAsync(userId);
            return Ok(collections);
        }

        /// <summary>
        /// Get a specific collection by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionResponseDto>> GetById(int userId, int id)
        {
            var collection = await _collectionService.GetByIdAsync(id, userId);

            if (collection == null)
            {
                return NotFound(new { message = "Collection not found" });
            }

            return Ok(collection);
        }

        /// <summary>
        /// Create a new collection
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CollectionResponseDto>> Create(int userId, [FromBody] CollectionCreateDto collectionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var collection = await _collectionService.CreateAsync(userId, collectionDto);

            if (collection == null)
            {
                return BadRequest(new { message = "Error creating collection" });
            }

            return CreatedAtAction(nameof(GetById), new { userId, id = collection.Id }, collection);
        }

        /// <summary>
        /// Update an existing collection
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CollectionResponseDto>> Update(int userId, int id, [FromBody] CollectionUpdateDto collectionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var collection = await _collectionService.UpdateAsync(id, userId, collectionDto);

            if (collection == null)
            {
                return NotFound(new { message = "Collection not found" });
            }

            return Ok(collection);
        }

        /// <summary>
        /// Delete a collection
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int userId, int id)
        {
            var result = await _collectionService.DeleteAsync(id, userId);

            if (!result)
            {
                return NotFound(new { message = "Collection not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Add a book to a collection
        /// </summary>
        [HttpPost("{collectionId}/books/{bookId}")]
        public async Task<ActionResult> AddBook(int userId, int collectionId, int bookId)
        {
            var result = await _collectionService.AddBookToCollectionAsync(collectionId, bookId, userId);

            if (!result)
            {
                return BadRequest(new { message = "Error adding book to collection" });
            }

            return Ok(new { message = "Book added to collection successfully" });
        }

        /// <summary>
        /// Remove a book from a collection
        /// </summary>
        [HttpDelete("{collectionId}/books/{bookId}")]
        public async Task<ActionResult> RemoveBook(int userId, int collectionId, int bookId)
        {
            var result = await _collectionService.RemoveBookFromCollectionAsync(collectionId, bookId, userId);

            if (!result)
            {
                return NotFound(new { message = "Book not found in collection" });
            }

            return NoContent();
        }

        /// <summary>
        /// Add an author to a collection
        /// </summary>
        [HttpPost("{collectionId}/authors/{authorId}")]
        public async Task<ActionResult> AddAuthor(int userId, int collectionId, int authorId)
        {
            var result = await _collectionService.AddAuthorToCollectionAsync(collectionId, authorId, userId);

            if (!result)
            {
                return BadRequest(new { message = "Error adding author to collection or author already exists in collection" });
            }

            return Ok(new { message = "Author added to collection successfully" });
        }

        /// <summary>
        /// Remove an author from a collection
        /// </summary>
        [HttpDelete("{collectionId}/authors/{authorId}")]
        public async Task<ActionResult> RemoveAuthor(int userId, int collectionId, int authorId)
        {
            var result = await _collectionService.RemoveAuthorFromCollectionAsync(collectionId, authorId, userId);

            if (!result)
            {
                return NotFound(new { message = "Author not found in collection" });
            }

            return NoContent();
        }

        /// <summary>
        /// Reorder books in a collection
        /// </summary>
        [HttpPut("{collectionId}/books/reorder")]
        public async Task<ActionResult> ReorderBooks(int userId, int collectionId, [FromBody] ReorderBooksDto reorderDto)
        {
            var result = await _collectionService.ReorderBooksAsync(collectionId, userId, reorderDto);

            if (!result)
            {
                return NotFound(new { message = "Collection not found or unauthorized" });
            }

            return NoContent();
        }
    }
}
