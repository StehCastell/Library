using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
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
        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(new List<Collection>());
        }

        // POST: Collections/Create
        [HttpPost]
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
        [HttpPut]
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
        [HttpDelete]
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
        [HttpGet]
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
        [HttpGet]
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

        // POST: Collections/AddBook/{collectionId}/{bookId}
        [HttpPost]
        public async Task<IActionResult> AddBook(int collectionId, int bookId)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            var userId = GetUserId();
            var success = await _apiService.AddBookToCollectionAsync(userId, collectionId, bookId);

            if (!success)
            {
                return BadRequest(new { message = "Error adding book to collection" });
            }

            return Ok(new { message = "Book added to collection successfully" });
        }

        // DELETE: Collections/RemoveBook/{collectionId}/{bookId}
        [HttpDelete]
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
    }
}
