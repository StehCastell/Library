using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Se já estiver logado, redireciona para books
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return LocalRedirect("/books");
            }

            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _apiService.LoginAsync(model);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email ou password incorretos");
                return View(model);
            }

            // Save na sessão
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserTheme", user.Theme ?? "light");

            return LocalRedirect("/books");
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            // Se já estiver logado, redireciona para books
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return LocalRedirect("/books");
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _apiService.RegisterAsync(model);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Este email já está cadastrado ou ocorreu um erro");
                return View(model);
            }

            // Login automático após registro
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserTheme", user.Theme ?? "light");

            return LocalRedirect("/books");
        }

        // POST: Account/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return LocalRedirect("/account/login");
        }
    }
}
