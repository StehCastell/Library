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

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Users/register", content);

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

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Users/login", content);

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
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/Users/{id}");

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

        // ========== BOOKS ==========

        public async Task<List<Book>> GetBooksAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/Books");

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
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/{userId}/Books/{bookId}");

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

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/{userId}/Books", content);

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

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/users/{userId}/Books/{bookId}", content);

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
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/users/{userId}/Books/{bookId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
