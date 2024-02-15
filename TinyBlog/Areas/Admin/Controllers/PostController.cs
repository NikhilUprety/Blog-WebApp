using Microsoft.AspNetCore.Mvc;
using TinyBlog.ViewModel;
using Microsoft.AspNetCore.Authorization;
using TinyBlog.Data;
using Microsoft.AspNetCore.Identity;
using TinyBlog.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Drawing;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TinyBlog.Utilities;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostVMcs vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new post();

            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserID = loggedInUser!.Id;

            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.PostTable!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Post Created Successfully");
            return RedirectToAction("Index");
        }




        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }







    }
}
