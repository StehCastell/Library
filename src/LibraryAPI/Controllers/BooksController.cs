using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// List all books for a user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAll(int userId)
        {
            var books = await _bookService.GetByUserIdAsync(userId);
            return Ok(books);
        }

        /// <summary>
        /// Get a specific book by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseDto>> GetById(int userId, int id)
        {
            var book = await _bookService.GetByIdAsync(id, userId);

            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(book);
        }

        /// <summary>
        /// Create a new book
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> Create(int userId, [FromBody] BookCreateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _bookService.CreateAsync(userId, bookDto);

            if (book == null)
            {
                return BadRequest(new { message = "Error creating book" });
            }

            return CreatedAtAction(nameof(GetById), new { userId, id = book.Id }, book);
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponseDto>> Update(int userId, int id, [FromBody] BookUpdateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _bookService.UpdateAsync(id, userId, bookDto);

            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(book);
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int userId, int id)
        {
            var result = await _bookService.DeleteAsync(id, userId);

            if (!result)
            {
                return NotFound(new { message = "Book not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Add an author to a book
        /// </summary>
        [HttpPost("{bookId}/authors/{authorId}")]
        public async Task<ActionResult> AddAuthor(int userId, int bookId, int authorId)
        {
            var result = await _bookService.AddAuthorToBookAsync(bookId, authorId, userId);

            if (!result)
            {
                return BadRequest(new { message = "Error adding author to book or author already exists" });
            }

            return Ok(new { message = "Author added to book successfully" });
        }

        /// <summary>
        /// Remove an author from a book
        /// </summary>
        [HttpDelete("{bookId}/authors/{authorId}")]
        public async Task<ActionResult> RemoveAuthor(int userId, int bookId, int authorId)
        {
            var result = await _bookService.RemoveAuthorFromBookAsync(bookId, authorId, userId);

            if (!result)
            {
                return NotFound(new { message = "Author not found in book" });
            }

            return NoContent();
        }

        /// <summary>
        /// Get all authors of a book
        /// </summary>
        [HttpGet("{bookId}/authors")]
        public async Task<ActionResult> GetBookAuthors(int userId, int bookId)
        {
            var authors = await _bookService.GetBookAuthorsAsync(bookId, userId);

            if (authors == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(authors);
        }
    }
}
