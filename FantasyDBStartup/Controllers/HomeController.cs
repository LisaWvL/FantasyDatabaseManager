
using Microsoft.AspNetCore.Mvc;

namespace FantasyDBStartup.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
