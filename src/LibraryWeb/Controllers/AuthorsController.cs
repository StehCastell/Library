using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    [Route("authors")]
    public class AuthorsController : Controller
    {
        private readonly IApiService _apiService;

        public AuthorsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // GET: Authors
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return LocalRedirect("/account/login");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = GetUserId();
            ViewBag.UserTheme = HttpContext.Session.GetString("UserTheme") ?? "light";
            return View(new List<Author>());
        }

        // POST: Authors/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Author author)
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return Unauthorized(new { message = "User not logged in" });
                }

                if (author == null)
                {
                    return BadRequest(new { message = "Author data is required" });
                }

                if (string.IsNullOrWhiteSpace(author.Name))
                {
                    return BadRequest(new { message = "Author name is required" });
                }

                var userId = GetUserId();
                var newAuthor = await _apiService.CreateAuthorAsync(userId, author);

                if (newAuthor == null)
                {
                    return BadRequest(new { message = "Error creating author" });
                }

                return Ok(newAuthor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT: Authors/Update/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Author author)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var updatedAuthor = await _apiService.UpdateAuthorAsync(userId, id, author);

            if (updatedAuthor == null)
            {
                return NotFound(new { message = "Author not found" });
            }

            return Ok(updatedAuthor);
        }

        // DELETE: Authors/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var success = await _apiService.DeleteAuthorAsync(userId, id);

            if (!success)
            {
                return NotFound(new { message = "Author not found" });
            }

            return Ok(new { message = "Author successfully deleted" });
        }

        // GET: Authors/Get/5
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var author = await _apiService.GetAuthorByIdAsync(userId, id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // GET: Authors/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var authors = await _apiService.GetAuthorsAsync(userId);

            return Ok(authors ?? new List<Author>());
        }
    }
}
