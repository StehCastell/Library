using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    [Route("[controller]")]
    public class CollectionsController : Controller
    {
        private readonly IApiService _apiService;

        public CollectionsController(IApiService apiService)
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

        // GET: Collections
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = GetUserId();
            ViewBag.UserTheme = HttpContext.Session.GetString("UserTheme") ?? "light";
            return View(new List<Collection>());
        }

        // POST: Collections/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Collection collection)
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return Unauthorized(new { message = "User not logged in" });
                }

                if (collection == null)
                {
                    return BadRequest(new { message = "Collection data is required" });
                }

                if (string.IsNullOrWhiteSpace(collection.Name))
                {
                    return BadRequest(new { message = "Collection name is required" });
                }

                var userId = GetUserId();
                collection.UserId = userId;

                var newCollection = await _apiService.CreateCollectionAsync(userId, collection);

                if (newCollection == null)
                {
                    return BadRequest(new { message = "Error creating collection" });
                }

                return Ok(newCollection);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT: Collections/Update/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Collection collection)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            collection.UserId = userId;

            var updatedCollection = await _apiService.UpdateCollectionAsync(userId, id, collection);

            if (updatedCollection == null)
            {
                return NotFound(new { message = "Collection not found" });
            }

            return Ok(updatedCollection);
        }

        // DELETE: Collections/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var success = await _apiService.DeleteCollectionAsync(userId, id);

            if (!success)
            {
                return NotFound(new { message = "Collection not found" });
            }

            return Ok(new { message = "Collection successfully deleted" });
        }

        // GET: Collections/Get/5
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var collection = await _apiService.GetCollectionByIdAsync(userId, id);

            if (collection == null)
            {
                return NotFound();
            }

            return Ok(collection);
        }

        // GET: Collections/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var collections = await _apiService.GetCollectionsAsync(userId);

            return Ok(collections ?? new List<Collection>());
        }

        // POST: Collections/AddBook
        [HttpPost("AddBook/{collectionId}/{bookId}")]
        public async Task<IActionResult> AddBook(int collectionId, int bookId)
        {
            Console.WriteLine($"📤 AddBook called: collectionId={collectionId}, bookId={bookId}");

            if (!IsLoggedIn())
            {
                Console.WriteLine("❌ User not logged in");
                return Unauthorized();
            }

            var userId = GetUserId();
            Console.WriteLine($"👤 UserId: {userId}");

            var success = await _apiService.AddBookToCollectionAsync(userId, collectionId, bookId);
            Console.WriteLine($"✅ AddBookToCollectionAsync result: {success}");

            if (!success)
            {
                Console.WriteLine("❌ Failed to add book to collection");
                return BadRequest(new { message = "Error adding book to collection" });
            }

            return Ok(new { message = "Book added to collection successfully" });
        }

        // DELETE: Collections/RemoveBook
        [HttpDelete("RemoveBook/{collectionId}/{bookId}")]
        public async Task<IActionResult> RemoveBook(int collectionId, int bookId)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var success = await _apiService.RemoveBookFromCollectionAsync(userId, collectionId, bookId);

            if (!success)
            {
                return NotFound(new { message = "Book not found in collection" });
            }

            return Ok(new { message = "Book removed from collection successfully" });
        }

        // POST: Collections/AddAuthor
        [HttpPost("AddAuthor/{collectionId}/{authorId}")]
        public async Task<IActionResult> AddAuthor(int collectionId, int authorId)
        {
            Console.WriteLine($"📤 AddAuthor called: collectionId={collectionId}, authorId={authorId}");

            if (!IsLoggedIn())
            {
                Console.WriteLine("❌ User not logged in");
                return Unauthorized();
            }

            var userId = GetUserId();
            Console.WriteLine($"👤 UserId: {userId}");

            var success = await _apiService.AddAuthorToCollectionAsync(userId, collectionId, authorId);
            Console.WriteLine($"✅ AddAuthorToCollectionAsync result: {success}");

            if (!success)
            {
                Console.WriteLine("❌ Failed to add author to collection");
                return BadRequest(new { message = "Error adding author to collection" });
            }

            return Ok(new { message = "Author added to collection successfully" });
        }

        // DELETE: Collections/RemoveAuthor
        [HttpDelete("RemoveAuthor/{collectionId}/{authorId}")]
        public async Task<IActionResult> RemoveAuthor(int collectionId, int authorId)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var success = await _apiService.RemoveAuthorFromCollectionAsync(userId, collectionId, authorId);

            if (!success)
            {
                return NotFound(new { message = "Author not found in collection" });
            }

            return Ok(new { message = "Author removed from collection successfully" });
        }
    }
}
