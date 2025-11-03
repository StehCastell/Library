using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var books = await _apiService.GetBooksAsync(userId);

            // ✅ CORREÇÃO: Garantir que sempre passe uma lista, nunca null
            if (books == null)
            {
                books = new List<Book>();
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(books);
        }

        // POST: Books/Create
        [HttpPost]
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
        [HttpPut]
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
        [HttpDelete]
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
        [HttpGet]
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

            return Ok(book);
        }
    }
}
