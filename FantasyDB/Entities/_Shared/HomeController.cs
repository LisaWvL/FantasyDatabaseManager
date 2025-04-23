using Microsoft.AspNetCore.Mvc;

namespace FantasyDB.Entities._Shared
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
