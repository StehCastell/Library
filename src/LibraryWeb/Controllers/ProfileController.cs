using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IApiService _apiService;

        public ProfileController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (userId == null || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            // Buscar dados atualizados do usuário da API
            var user = await _apiService.GetUserByIdAsync(userId.Value);

            var model = new ProfileViewModel
            {
                Name = userName,
                Email = userEmail,
                ProfileImage = user?.ProfileImage
            };

            ViewBag.UserId = userId.Value;
            ViewBag.UserTheme = HttpContext.Session.GetString("UserTheme") ?? "light";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.UserId = userId.Value;
                return View("Index", model);
            }

            var updatedUser = await _apiService.UpdateUserProfileAsync(userId.Value, model);

            if (updatedUser == null)
            {
                ModelState.AddModelError("", "Não foi possível atualizar o perfil. O email pode já estar em uso.");
                ViewBag.UserId = userId.Value;
                return View("Index", model);
            }

            // Atualizar sessão com novos dados
            HttpContext.Session.SetString("UserName", updatedUser.Name);
            HttpContext.Session.SetString("UserEmail", updatedUser.Email);

            if (!string.IsNullOrEmpty(updatedUser.ProfileImage))
            {
                HttpContext.Session.SetString("UserProfileImage", updatedUser.ProfileImage);
            }

            TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
