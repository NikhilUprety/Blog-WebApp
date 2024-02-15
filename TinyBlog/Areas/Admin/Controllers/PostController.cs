using Microsoft.AspNetCore.Mvc;
using TinyBlog.ViewModel;
using Microsoft.AspNetCore.Authorization;
using TinyBlog.Data;
using Microsoft.AspNetCore.Identity;
using TinyBlog.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Drawing;
using System.Reflection;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,
            INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            context = _context;
            _userManager = userManager;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
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
            if (!ModelState.IsValid) { }
            {
                return View(vm);
            }

            var logedInUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var post = new post();

            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserID = logedInUser.Id;
            
            if(post.Title != null)
            {
                string slug = vm.Title.Trim();
                slug=slug.Replace("","_");
                post.slug = slug+"_"+Guid.NewGuid();
            }

            if(vm.Thumbnail!= null) {

                post.ThumbnailUrl=UploadImage(vm.Thumbnail);
            }

            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("post saved successfully");
            return RedirectToAction("Index","Post",new { area ="Admin"});
        }

       private string UploadImage(IFormFile file)
        {
            var uniquefilename = "";
            var folderpath = Path.Combine(_webHostEnvironment.WebRootPath,"Thumbnail");
            uniquefilename = Guid.NewGuid().ToString()+"_"+file.FileName;
            var filePath = Path.Combine(folderpath,uniquefilename);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniquefilename;
        }



         


    } 
}
