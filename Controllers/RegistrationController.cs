using Microsoft.AspNetCore.Mvc;

namespace LerningAsp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Registr()
        {
            return View("Registr");
        }
        public IActionResult Loging()
        {
            return View();
        }
    }
}
