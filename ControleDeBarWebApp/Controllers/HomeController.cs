using Microsoft.AspNetCore.Mvc;

namespace ControleDeBarWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
