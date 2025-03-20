using Microsoft.AspNetCore.Mvc;

namespace RealEStateProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
