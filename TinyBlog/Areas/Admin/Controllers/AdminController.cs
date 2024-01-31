using Microsoft.AspNetCore.Mvc;

namespace TinyBlog.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
