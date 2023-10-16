using Microsoft.AspNetCore.Mvc;

namespace LoginSystem.Controllers
{
    public class PretenderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}