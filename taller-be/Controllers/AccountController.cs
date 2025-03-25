using Microsoft.AspNetCore.Mvc;

namespace tu_app.Controllers
{
    public class AccountController : Controller
    {
        // Acción para mostrar el formulario de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
