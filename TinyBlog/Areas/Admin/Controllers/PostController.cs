using Microsoft.AspNetCore.Mvc;
using TinyBlog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TinyBlog.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        [Area("Admin")]
        [Authorize] 
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVMcs());
        }
    }
}
