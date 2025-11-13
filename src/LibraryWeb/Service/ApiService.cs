using LibraryWeb.Models;
using Newtonsoft.Json;
using System.Text;

namespace LibraryWeb.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
        }

        // ========== USUÁRIOS ==========

        public async Task<User?> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> LoginAsync(LoginViewModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserThemeAsync(int userId, string theme)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { Theme = theme });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/theme", content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> UpdateUserProfileAsync(int userId, ProfileViewModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/profile", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, PasswordViewModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/password", content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ========== BOOKS ==========

        public async Task<List<Book>> GetBooksAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/books");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(responseContent) ?? new List<Book>();
                }

                return new List<Book>();
            }
            catch
            {
                return new List<Book>();
            }
        }

        public async Task<Book?> GetBookByIdAsync(int userId, int bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Book>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Book?> CreateBookAsync(int userId, Book book)
        {
            try
            {
                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/{userId}/books", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Book>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Book?> UpdateBookAsync(int userId, int bookId, Book book)
        {
            try
            {
                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Book>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteBookAsync(int userId, int bookId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Author>> GetBookAuthorsAsync(int bookId, int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}/authors");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Author>>(json) ?? new List<Author>();
                }
                return new List<Author>();
            }
            catch
            {
                return new List<Author>();
            }
        }

        public async Task<bool> AddAuthorToBookAsync(int userId, int bookId, int authorId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}/authors/{authorId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAuthorFromBookAsync(int userId, int bookId, int authorId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/books/{bookId}/authors/{authorId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ========== AUTHORS ==========

        public async Task<List<Author>> GetAuthorsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/authors");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Author>>(responseContent) ?? new List<Author>();
                }

                return new List<Author>();
            }
            catch
            {
                return new List<Author>();
            }
        }

        public async Task<Author?> GetAuthorByIdAsync(int userId, int authorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/authors/{authorId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Author>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Author?> CreateAuthorAsync(int userId, Author author)
        {
            try
            {
                var json = JsonConvert.SerializeObject(author);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/{userId}/authors", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Author>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Author?> UpdateAuthorAsync(int userId, int authorId, Author author)
        {
            try
            {
                var json = JsonConvert.SerializeObject(author);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/authors/{authorId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Author>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int userId, int authorId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/authors/{authorId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ========== COLLECTIONS ==========

        public async Task<List<Collection>> GetCollectionsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/collections");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Collection>>(responseContent) ?? new List<Collection>();
                }

                return new List<Collection>();
            }
            catch
            {
                return new List<Collection>();
            }
        }

        public async Task<Collection?> GetCollectionByIdAsync(int userId, int collectionId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/collections/{collectionId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Collection>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Collection?> CreateCollectionAsync(int userId, Collection collection)
        {
            try
            {
                var json = JsonConvert.SerializeObject(collection);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/{userId}/collections", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Collection>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Collection?> UpdateCollectionAsync(int userId, int collectionId, Collection collection)
        {
            try
            {
                var json = JsonConvert.SerializeObject(collection);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/collections/{collectionId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Collection>(responseContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteCollectionAsync(int userId, int collectionId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/collections/{collectionId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddBookToCollectionAsync(int userId, int collectionId, int bookId)
        {
            try
            {
                var url = $"{_baseUrl}/api/users/{userId}/collections/{collectionId}/books/{bookId}";
                Console.WriteLine($"🌐 Calling API: POST {url}");

                var response = await _httpClient.PostAsync(url, null);
                Console.WriteLine($"📡 API Response: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ API Error: {error}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in AddBookToCollectionAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveBookFromCollectionAsync(int userId, int collectionId, int bookId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/collections/{collectionId}/books/{bookId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddAuthorToCollectionAsync(int userId, int collectionId, int authorId)
        {
            try
            {
                var url = $"{_baseUrl}/api/users/{userId}/collections/{collectionId}/authors/{authorId}";
                Console.WriteLine($"🌐 Calling API: POST {url}");

                var response = await _httpClient.PostAsync(url, null);
                Console.WriteLine($"📡 API Response: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ API Error: {error}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in AddAuthorToCollectionAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveAuthorFromCollectionAsync(int userId, int collectionId, int authorId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/collections/{collectionId}/authors/{authorId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ReorderBooksInCollectionAsync(int userId, int collectionId, object reorderDto)
        {
            try
            {
                var url = $"{_baseUrl}/api/users/{userId}/collections/{collectionId}/books/reorder";
                Console.WriteLine($"🌐 Calling API: PUT {url}");

                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(reorderDto),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PutAsync(url, content);
                Console.WriteLine($"📡 API Response: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ API Error: {error}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in ReorderBooksInCollectionAsync: {ex.Message}");
                return false;
            }
        }
    }
}
