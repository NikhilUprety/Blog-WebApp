using Microsoft.AspNetCore.Mvc;

namespace TinyBlog.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
