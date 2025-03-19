using Microsoft.AspNetCore.Mvc;

namespace RealEStateProject.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
    }
}
