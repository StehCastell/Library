using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly IApiService _apiService;

        public BooksController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Verificar se está logado
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // GET: Books
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return LocalRedirect("/account/login");
            }

            var userId = GetUserId();
            var books = await _apiService.GetBooksAsync(userId);

            // ✅ CORREÇÃO: Garantir que sempre passe uma lista, nunca null
            if (books == null)
            {
                books = new List<Book>();
            }

            // Carregar autores para cada livro
            foreach (var book in books)
            {
                book.Authors = await _apiService.GetBookAuthorsAsync(book.Id, userId);
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = userId;
            ViewBag.UserTheme = HttpContext.Session.GetString("UserTheme") ?? "light";

            return View(books);
        }

        // POST: Books/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            try
            {
                // Log para debug
                Console.WriteLine("📤 Create Book called");
                Console.WriteLine($"📝 Book received: Title={book?.Title}, Author={book?.Author}");

                if (!IsLoggedIn())
                {
                    Console.WriteLine("❌ User not logged in");
                    return Unauthorized(new { message = "User not logged in" });
                }

                if (book == null)
                {
                    Console.WriteLine("❌ Book is null");
                    return BadRequest(new { message = "Book data is required" });
                }

                // Validação manual
                if (string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }
                if (string.IsNullOrWhiteSpace(book.Author))
                {
                    return BadRequest(new { message = "Author is required" });
                }
                if (string.IsNullOrWhiteSpace(book.Genre))
                {
                    return BadRequest(new { message = "Genre is required" });
                }
                if (book.Pages <= 0)
                {
                    return BadRequest(new { message = "Pages must be greater than 0" });
                }
                if (string.IsNullOrWhiteSpace(book.Type))
                {
                    return BadRequest(new { message = "Type is required" });
                }
                if (string.IsNullOrWhiteSpace(book.Status))
                {
                    return BadRequest(new { message = "Status is required" });
                }

                var userId = GetUserId();
                Console.WriteLine($"👤 User ID: {userId}");

                // Adicionar UserId ao book
                book.UserId = userId;

                Console.WriteLine($"📤 Calling API Service...");
                var newBook = await _apiService.CreateBookAsync(userId, book);

                if (newBook == null)
                {
                    Console.WriteLine("❌ API returned null");
                    return BadRequest(new { message = "Error creating book - API returned null" });
                }

                Console.WriteLine($"✅ Book created successfully: ID={newBook.Id}");
                return Ok(newBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in Create: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT: Books/Update/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();

            // ✅ CORREÇÃO: Adicionar UserId ao book antes de enviar para a API
            book.UserId = userId;

            var bookAtualizado = await _apiService.UpdateBookAsync(userId, id, book);

            if (bookAtualizado == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(bookAtualizado);
        }

        // DELETE: Books/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var sucess = await _apiService.DeleteBookAsync(userId, id);

            if (!sucess)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(new { message = "Book sucess delete" });
        }

        // GET: Books/Get/5
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var book = await _apiService.GetBookByIdAsync(userId, id);

            if (book == null)
            {
                return NotFound();
            }

            // Carregar autores do livro
            book.Authors = await _apiService.GetBookAuthorsAsync(book.Id, userId);

            return Ok(book);
        }

        // GET: Books/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var books = await _apiService.GetBooksAsync(userId);

            return Ok(books ?? new List<Book>());
        }

        // POST: Books/AddAuthor/5/10
        [HttpPost("AddAuthor/{bookId}/{authorId}")]
        public async Task<IActionResult> AddAuthor(int bookId, int authorId)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var result = await _apiService.AddAuthorToBookAsync(userId, bookId, authorId);

            if (!result)
            {
                return BadRequest(new { message = "Error adding author to book" });
            }

            return Ok(new { message = "Author added to book successfully" });
        }

        // DELETE: Books/RemoveAuthor/5/10
        [HttpDelete("RemoveAuthor/{bookId}/{authorId}")]
        public async Task<IActionResult> RemoveAuthor(int bookId, int authorId)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var result = await _apiService.RemoveAuthorFromBookAsync(userId, bookId, authorId);

            if (!result)
            {
                return NotFound(new { message = "Author not found in book" });
            }

            return Ok(new { message = "Author removed from book successfully" });
        }

        // PUT: Books/UpdateTheme
        [HttpPut("UpdateTheme")]
        public async Task<IActionResult> UpdateTheme([FromBody] ThemeUpdateRequest request)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var result = await _apiService.UpdateUserThemeAsync(userId, request.Theme);

            if (!result)
            {
                return BadRequest(new { message = "Error updating theme" });
            }

            // Update session
            HttpContext.Session.SetString("UserTheme", request.Theme);

            return Ok(new { message = "Theme updated successfully" });
        }
    }

    public class ThemeUpdateRequest
    {
        public string Theme { get; set; } = string.Empty;
    }
}
