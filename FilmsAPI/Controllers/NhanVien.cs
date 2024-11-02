using Microsoft.AspNetCore.Mvc;

namespace FilmsAPI.Controllers
{
    public class NhanVien : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
