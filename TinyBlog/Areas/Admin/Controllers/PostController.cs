using Microsoft.AspNetCore.Mvc;

namespace TinyBlog.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
