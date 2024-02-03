using Microsoft.AspNetCore.Mvc;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("login")]
        public IActionResult login()
        {
            return View();
        }
    }
}
