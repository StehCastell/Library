using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Redirecionar para login se não estiver logado
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Se estiver logado, redirecionar para books
            return RedirectToAction("Index", "Books");
        }
    }
}
