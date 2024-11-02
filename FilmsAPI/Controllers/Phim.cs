using Microsoft.AspNetCore.Mvc;

namespace FilmsAPI.Controllers
{
    public class Phim : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
