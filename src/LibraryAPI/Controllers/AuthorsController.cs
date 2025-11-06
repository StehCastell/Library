using LibraryAPI.DTOs;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// List all authors for a user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAll(int userId)
        {
            var authors = await _authorService.GetByUserIdAsync(userId);
            return Ok(authors);
        }

        /// <summary>
        /// Get a specific author by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponseDto>> GetById(int userId, int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new { message = "Author not found" });
            }

            return Ok(author);
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AuthorResponseDto>> Create(int userId, [FromBody] AuthorCreateDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _authorService.CreateAsync(userId, authorDto);

            if (author == null)
            {
                return BadRequest(new { message = "Error creating author" });
            }

            return CreatedAtAction(nameof(GetById), new { userId, id = author.Id }, author);
        }

        /// <summary>
        /// Update an existing author
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorResponseDto>> Update(int userId, int id, [FromBody] AuthorUpdateDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _authorService.UpdateAsync(id, userId, authorDto);

            if (author == null)
            {
                return NotFound(new { message = "Author not found" });
            }

            return Ok(author);
        }

        /// <summary>
        /// Delete an author
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int userId, int id)
        {
            var result = await _authorService.DeleteAsync(id, userId);

            if (!result)
            {
                return NotFound(new { message = "Author not found" });
            }

            return NoContent();
        }
    }
}
