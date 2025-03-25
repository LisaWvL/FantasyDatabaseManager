using Microsoft.AspNetCore.Mvc;

namespace FantasyDB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
